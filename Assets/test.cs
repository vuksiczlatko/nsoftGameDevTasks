using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class test : MonoBehaviour
{
    //Size of our maze
    private const int size = 15;
    //Cube prefab to spawn
    public GameObject cubePrefab;
    //Matrix holding all our cube gameobjects
    public GameObject[][] matrix;
    //Population matrix of indexes representing a copy of our maze, 0 = wall, 1 = free, 2 = visited, 3 = end
    public int[][] populationMatrix;
    public int[][] solvedMazeData;
    //Since we have 2 openings and closings we store those 4 indexes here
    int[] arrayOfOpenings;
    private static LinkedList<Coordinate> q = new LinkedList<Coordinate>();
    public Material closedMat;
    public Material openMat;
    public Material solvedMat;

    [Range(2f, 30f)]
    public float wavelength;
    [Range(0.1f, 10f)]
    public float timelength;
    public bool useJobSystem = false;


    void Start()
    {
        closedMat.color = Color.black;
        openMat.color = Color.white;
        solvedMat.color = Color.green;
        matrix = new GameObject[size][];
        populationMatrix = new int[size][];
        GenerateMap();
        MakeMaze();
        PrintOut();
        //SaveMazeTexture();
        string printMaterial = "";
        for(int i = 0; i < 15; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                printMaterial += " " + populationMatrix[i][j];
            }
            printMaterial += "\n";
        }
        //print out the population matrix
        Debug.Log(printMaterial);
        //SaveMazeTexture();
        LoadMazeTexture();
        printMaterial = "";
        for(int i = 0; i < 15; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                printMaterial += " " + solvedMazeData[i][j];
            }
            printMaterial += "\n";
        }
        Debug.Log("Solved Maze:\n" + printMaterial);
        {
            float step = 2f / size;
            Vector3 scale = Vector3.one * step;
            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    matrix[i][j].transform.localScale = scale;
                }

            }
        }
        StartCoroutine(AnimateMaze());
    }

    private void Update()
    {
        
    }

    private void SaveMazeTexture()
    {
        Texture2D mazeTexture = new Texture2D(15, 15, TextureFormat.RGBA32, false);

        for(int i = 0; i < 15; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                if(matrix[i][j].GetComponent<MeshRenderer>().material.color == Color.black)
                    mazeTexture.SetPixel(i, j, Color.black);
                else if(matrix[i][j].GetComponent<MeshRenderer>().material.color == Color.white)
                    mazeTexture.SetPixel(i, j, Color.white);
                else
                    mazeTexture.SetPixel(i, j, Color.green);
            }
        }
        mazeTexture.Apply();
        File.WriteAllBytes("Assets/Mazetexture.png", ImageConversion.EncodeToPNG(mazeTexture));
    }
    private void LoadMazeTexture()
    {
        Texture2D mazeTex = new Texture2D(15,15,TextureFormat.RGBA32,false);
        ImageConversion.LoadImage(mazeTex, File.ReadAllBytes("Assets/Mazetexture.png"));
        for(int i = 0; i < 15; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                matrix[i][j].GetComponent<MeshRenderer>().material.color = mazeTex.GetPixel(i, j);
            }
        }
    }
    private void GenerateMap()
    {
        //Generate a map of cubes
        for(int i = 0; i < size; i++)
        {
            matrix[i] = new GameObject[size];
            populationMatrix[i] = new int[size];
            for(int j = 0; j < size; j++)
            {
                matrix[i][j] = Instantiate(cubePrefab, new Vector3(j * 1.1f,1,i * -1.1f), new Quaternion(1, 1, 1, 1));
                matrix[i][j].GetComponent<MeshRenderer>().material = openMat;
                populationMatrix[i][j] = 0;
                matrix[i][j].name = "Cube [" + i + "][" + j + "]";
            }
        }
        CreateOpenings();
    }
    private int[] ChooseOpeningClosing()
    {
        //Randomly choose opening and closing cells from the first and last row
        int[] arrayOfOpenings = new int[4];
        List<int> arrayOfNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        for(int i = 0; i < 4; i++)
        {
            int index = (int)Random.Range(0, arrayOfNumbers.Count);
            arrayOfOpenings[i] = arrayOfNumbers[index];
            arrayOfNumbers.RemoveAt(index);
            if(index == 0)
                arrayOfNumbers.RemoveAt(index);
            else if(index == arrayOfNumbers.Count)
                arrayOfNumbers.RemoveAt(index - 1);
            else
            {
                arrayOfNumbers.RemoveAt(index);
                arrayOfNumbers.RemoveAt(index - 1);
            }
            if(i == 1)
            {
                for(int j = 0; j < arrayOfNumbers.Count; j++)
                {
                    arrayOfNumbers.RemoveAt(j);
                }
                arrayOfNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
            }
        }
        return arrayOfOpenings;
    }
    private void CreateOpenings()
    {
        //Color the cubes and map the openings in the populationMatrix
        arrayOfOpenings = ChooseOpeningClosing();
        for(int i = 0; i < 2; i++)
        {
            matrix[arrayOfOpenings[i]][0].GetComponent<MeshRenderer>().material = closedMat;
            populationMatrix[arrayOfOpenings[i]][0] = 1;
        }
        for(int i = 2; i < 4; i++)
        {
            matrix[arrayOfOpenings[i]][14].GetComponent<MeshRenderer>().material = closedMat;
            populationMatrix[arrayOfOpenings[i]][14] = 3;
        }
    }
    private void MakeMaze()
    {
        //Color the maze black for closed and white for open cells
        for(int i = 1; i < 14; i += 2)
        {
            for(int j = 1; j < 14; j++)
            {
                matrix[j][i].GetComponent<MeshRenderer>().material = closedMat;
                populationMatrix[j][i] = 1;
            }
        }
        //color the third row
        for(int i = 0; i < 3; i++)
        {
            bool found = false;

            do
            {
                int randomNumb = (int)Random.Range(1, 14);
                if(randomNumb == arrayOfOpenings[0] || randomNumb == arrayOfOpenings[1] ||
                    matrix[randomNumb + 1][2].GetComponent<MeshRenderer>().material == openMat ||
                    matrix[randomNumb - 1][2].GetComponent<MeshRenderer>().material == openMat)
                    found = true;
                else
                {
                    matrix[randomNumb][2].GetComponent<MeshRenderer>().material = closedMat;
                    populationMatrix[randomNumb][2] = 1;

                    found = false;
                }
            } while(found);
        }
        //color the rows inbetween
        for(int i = 4; i < 11; i += 2)
        {
            for(int j = 0; j < 3; j++)
            {
                bool found = false;
                do
                {
                    int randomNumb = (int)Random.Range(1, 14);
                    if(matrix[randomNumb][i - 1].GetComponent<MeshRenderer>().material == openMat &&
                        matrix[randomNumb][i - 2].GetComponent<MeshRenderer>().material == openMat)
                        found = true;
                    else if(matrix[randomNumb + 1][i].GetComponent<MeshRenderer>().material == openMat ||
                        matrix[randomNumb - 1][i].GetComponent<MeshRenderer>().material == openMat)
                        found = true;
                    else
                    {
                        matrix[randomNumb][i].GetComponent<MeshRenderer>().material = closedMat;
                        populationMatrix[randomNumb][i] = 1;

                        found = false;
                    }
                } while(found);
            }
        }
        //color the 12'th row
        for(int j = 0; j < 3; j++)
        {
            bool found = false;
            do
            {
                int randomNumb = (int)Random.Range(1, 14);
                if(matrix[randomNumb][11].GetComponent<MeshRenderer>().material == openMat &&
                    matrix[randomNumb][10].GetComponent<MeshRenderer>().material == openMat)
                    found = true;
                else if(matrix[randomNumb][14].GetComponent<MeshRenderer>().material == openMat)
                    found = true;
                else if(matrix[randomNumb + 1][12].GetComponent<MeshRenderer>().material == openMat ||
                    matrix[randomNumb - 1][12].GetComponent<MeshRenderer>().material == openMat)
                    found = true;
                else
                {
                    matrix[randomNumb][12].GetComponent<MeshRenderer>().material = closedMat;
                    populationMatrix[randomNumb][12] = 1;

                    found = false;
                }
            } while(found);
        }


    }
    private Coordinate getPathBFS(int x, int y)
    {
        q.AddLast(new Coordinate(x, y, null));
        while(q.Count != 0)
        {
            Coordinate p = q.First.Value;
            q.RemoveFirst();
            if(populationMatrix[p.x][p.y] == 3)
            {
                return p;
            }
            if(isFree(p.x + 1, p.y))
            {
                populationMatrix[p.x][p.y] = 2;
                Coordinate nextP = new Coordinate(p.x + 1, p.y, p);
                q.AddLast(nextP);
            }
            if(isFree(p.x - 1, p.y))
            {
                populationMatrix[p.x][p.y] = 2;
                Coordinate nextP = new Coordinate(p.x - 1, p.y, p);
                q.AddLast(nextP);
            }
            if(isFree(p.x, p.y + 1))
            {
                populationMatrix[p.x][p.y] = 2;
                Coordinate nextP = new Coordinate(p.x, p.y + 1, p);
                q.AddLast(nextP);
            }
            if(isFree(p.x, p.y - 1))
            {
                populationMatrix[p.x][p.y] = 2;
                Coordinate nextP = new Coordinate(p.x, p.y - 1, p);
                q.AddLast(nextP);
            }
        }
        return null;
    }
    private bool isFree(int x, int y)
    {
        if((x >= 0 && x < size) && (y >= 0 && y < size) && (populationMatrix[x][y] == 1 || populationMatrix[x][y] == 3))
            return true;
        return false;
    }
    private void PrintOut()
    {
        string printMaterial = "";
        for(int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                printMaterial += " " + populationMatrix[i][j];
            }
            printMaterial += "\n";
        }
        //print out the population matrix
        Debug.Log(printMaterial);

        //Create coordinates, solve maze for both entries and check which path is shorter
        Coordinate[] coord = new Coordinate[2];
        coord[0] = getPathBFS(arrayOfOpenings[0], 0);
        coord[1] = getPathBFS(arrayOfOpenings[1], 0);
        int counter, counter1;
        counter = counter1 = 0;
        Coordinate[] coordTemp = new Coordinate[2];
        coordTemp[0] = coord[0];
        coordTemp[1] = coord[1];

        while(coordTemp[0].GetParent() != null)
        {
            counter++;
            coordTemp[0] = coordTemp[0].GetParent();
        }
        while(coordTemp[1].GetParent() != null)
        {
            counter1++;
            coordTemp[1] = coordTemp[1].GetParent();
        }


        //Lets check which one of those 2 paths is shorter, if they're the same print out the first one
        if(counter > counter1)
        {
            matrix[arrayOfOpenings[1]][0].GetComponent<MeshRenderer>().material = solvedMat;
            while(coord[0].GetParent() != null)
            {
                matrix[coord[0].x][coord[0].y].GetComponent<MeshRenderer>().material = solvedMat;
                coord[0] = coord[0].GetParent();
            }
        }
        else
        {
            matrix[arrayOfOpenings[0]][0].GetComponent<MeshRenderer>().material = solvedMat;
            while(coord[1].GetParent() != null)
            {
                matrix[coord[1].x][coord[1].y].GetComponent<MeshRenderer>().material = solvedMat;
                coord[1] = coord[1].GetParent();
            }
        }
        SaveSolvedMazeData();
    }
    private void SaveSolvedMazeData()
    {
        solvedMazeData = new int[size][];
        for(int i = 0; i < size; i++)
        {
            solvedMazeData[i] = new int[size];
            for(int j = 0; j < size; j++)
            {
                if(matrix[i][j].GetComponent<MeshRenderer>().material.color == Color.white)
                    solvedMazeData[i][j] = 0;
                else if(matrix[i][j].GetComponent<MeshRenderer>().material.color == Color.black)
                    solvedMazeData[i][j] = 1;
                else
                    solvedMazeData[i][j] = 2;
            }
        }
    }
    public IEnumerator AnimateMaze()
    {
        while(true)
        {
            float t = Time.time * timelength;
            GraphFunction f = Ripple;
            float step = 2f / size;
            for(int i = 0; i < size; i++)
            {
                
                if(useJobSystem)
                {
                    var theMatrix = new NativeArray<Vector3>(size, Allocator.Persistent);
                    for(int k = 0; k < size; k++)
                    {
                        theMatrix[k] = new Vector3(matrix[i][k].transform.position.x, matrix[i][k].transform.position.y, matrix[i][k].transform.position.z);
                    }
                    var job = new AnimateRipples()
                    {
                        i = i,
                        step = step,
                        t = t,
                        wavelength = wavelength,
                        passedArray = theMatrix
                    };

                    JobHandle jobHandle = job.Schedule();
                    
                    jobHandle.Complete();
                    theMatrix.Dispose();
                }
                else
                {
                    float v = (i + 0.5f) * step - 1f;
                    for(int j = 0; j < size; j++)
                    {
                        float u = (j + 0.5f) * step - 1f;
                        matrix[i][j].transform.localPosition = f(u, v, t);
                    }
                }
                
            }
            

            yield return new WaitForEndOfFrame();
        }
    }
    Vector3 Ripple(float x, float z, float t)
    {
        Vector3 p;
        float d = Mathf.Sqrt(x * x + z * z);
        p.x = x;
        p.y = Mathf.Sin(Mathf.PI * (4f * d - t));
        p.y /= wavelength + 10f * d;
        p.z = z;
        return p;
    }

    [BurstCompile]
    struct AnimateRipples : IJob
    {
        [ReadOnly] public int i;
        [ReadOnly] public float step, t, wavelength;
        public NativeArray<Vector3> passedArray;
        public void Execute()
        {
            float v = (i + 0.5f) * step - 1f;
            for(int j = 0; j < size; j++)
            {
                float u = (j + 0.5f) * step - 1f;
                Vector3 p;
                float d = Mathf.Sqrt(u * u + v * v);
                p.x = u;
                p.y = Mathf.Sin(Mathf.PI * (4f * d - t));
                p.y /= wavelength + 10f * d;
                p.z = v;
                passedArray[j] = p;
            }
        }
    }
}

public class Coordinate
{
    public Coordinate parent = null;
    public int x, y;
    public Coordinate(int cX, int cY)
    {
        x = cX;
        y = cY;
    }
    public Coordinate(int cX, int cY, Coordinate par)
    {
        x = cX;
        y = cY;
        parent = par;
    }
    public Coordinate GetParent()
    {
        return this.parent;
    }
    public string Print()
    {
        return "X|Y: " + this.x + "|" + this.y;
    }
}
