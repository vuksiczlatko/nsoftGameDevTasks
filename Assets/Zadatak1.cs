using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zadatak1 : MonoBehaviour
{
    public float time = 1;
    public float wavelenght = 1;
    public GameObject cubePrefab;
    public Test.Transform trans, trans1, trans2;
    public GameObject cube;

    public Vector3 inputValues;
    // Start is called before the first frame update
    void Start()
    {
        //Test.Vector3 vec = new Test.Vector3(-6, 8, 1);
        //Test.Vector3 vec1 = new Test.Vector3(5, 12, 1);
        //Test.Vector3[][] grid = vec.CreateGrid(15, 15, 1, 1, 1);

        //Create Grid without stiring
        //for(int i = 0; i < 100; i++)
        //{
        //    for(int j = 0; j < 100; j++)
        //    {
        //        Instantiate(cubePrefab, new Vector3(grid[i][j].x, grid[i][j].y, grid[i][j].z), new Quaternion(0, 0, 0, 0));
        //    }
        //}


        ////Create Grid with stiring
        //Test.Vector3[][] grid2 = vec.StirUpGrid(time, wavelenght, grid);
        //for(int i = 0; i < grid2.GetLength(0); i++)
        //{
        //    for(int j = 0; j < grid2.GetLength(0); j++)
        //    {
        //        Instantiate(cubePrefab, new Vector3(grid2[i][j].x, grid2[i][j].y, grid2[i][j].z), new Quaternion(1, 1, 1, 1));
        //    }
        //}


        //Test.Matrix4x4 testM1 = new Test.Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        //Test.Matrix4x4 testM2 = new Test.Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        //Test.Matrix4x4 testM3 = new Test.Matrix4x4(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
        //testM3.Multiply(testM2);
        //Test.Matrix4x4 testM4 = testM1.Transpose();
        //Debug.Log(testM1.PrintOut());
        //Debug.Log(testM4.PrintOut());


        //
        //Test.Matrix4x4 m = new Test.Matrix4x4();
        //Matrix4x4 uM = new Matrix4x4();
        //Debug.Log("Unity Matrix:\n" + uM.ToString());
        //Debug.Log("My Matrix:\n" + m.PrintOut());
        //Test.Vector3 vec = new Test.Vector3(inputValues.x, inputValues.y, inputValues.z);
        //m.Translate(vec);
        //m.Scale(vec);
        //
        //Debug.Log("Quaterion:" + Quaternion.Euler(inputValues.x, inputValues.y, inputValues.z));
        //Debug.Log("My Matrix:\n" + m.PrintOut());
        //
        //uM.SetTRS(Vector3.one, Quaternion.Euler(0, 0, 0), inputValues);
        //Debug.Log("Unity Matrix:\n" + uM.ToString());
        //Debug.Log(uM.rotation);

        //Test.Matrix4x4 q1 = new Test.Matrix4x4(3f);
        //Test.Matrix4x4 q2 = new Test.Matrix4x4();
        //Debug.Log(q1.PrintOut());
        //Debug.Log(q2.PrintOut());
        //q2.Multiply(q1);
        //Debug.Log(q2.PrintOut());
    }
}
namespace Test
{
    public class Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3()
        {
            x = y = z = 0;
        }
        public Vector3(float givenX, float givenY, float givenZ)
        {
            x = givenX;
            y = givenY;
            z = givenZ;
        }
        public Vector3(Vector3 second)
        {
            x = second.x;
            y = second.y;
            z = second.z;
        }


        public float ReturnDotProduct(Vector3 second)
        {
            return (this.x * second.x) + (this.y * second.y) + (this.z * second.z);
        }
        public Vector3 ReturnVectorProduct(Vector3 vec2)
        {
            float newX = (this.y * vec2.z) - (this.z * vec2.y);
            float newY = (this.z * vec2.x) - (this.x * vec2.z);
            float newZ = (this.x * vec2.y) - (this.y * vec2.x);
            return new Vector3(newX, newY, newZ);
        }
        public Vector3 NormalizeVector()
        {
            float unitVec = Mathf.Sqrt(((this.x * this.x) + (this.y * this.y) + (this.z * this.z)));
            return new Vector3((this.x / unitVec), (this.y / unitVec), (this.z / unitVec));
        }

        public Vector3[][] CreateGrid(int width, int lenght, int pointHeight, int deltaX, int deltaY)
        {
            Vector3[][] arrayOfDots = new Vector3[width][];
            for(int i = 0; i < width; i++)
            {
                arrayOfDots[i] = new Vector3[lenght];
                for(int j = 0; j < lenght; j++)
                {
                    arrayOfDots[i][j] = new Vector3((deltaX * i) - (width / 2), pointHeight, (deltaY * j) - (lenght / 2));
                }
            }

            return arrayOfDots;
        }
        public Vector3[][] StirUpGrid(float time, float wavelenght, Vector3[][] CreatedGrid)
        {
            Vector3[][] temp = new Vector3[CreatedGrid.GetLength(0)][];
            for(int i = temp.GetLength(0) - 1; i >= 0; i--)
            {
                temp[i] = new Vector3[CreatedGrid.GetLength(0)];
                for(int j = temp.GetLength(0) - 1; j >= 0; j--)
                {
                    temp[i][j] = CreatedGrid[i][j];
                    float offset = (temp[i][j].x * temp[i][j].x) + (temp[i][j].z * temp[i][j].z);
                    temp[i][j].y += wavelenght * Mathf.Sin(time + CreatedGrid[i][j].NormalizeVector().y * offset);
                }
            }

            return temp;
        }

        public static Vector3 operator +(Vector3 vec, Vector3 vec2)
        {
            float newX = vec.x + vec2.x;
            float newY = vec.y + vec2.y;
            float newZ = vec.z + vec2.z;
            return new Vector3(newX, newY, newZ);
        }
        public float Magnitude()
        {
            return Mathf.Sqrt((this.x * this.x) + (this.y * this.y) + (this.z * this.z));
        }
    }

    public class Quaterion
    {
        public float w;
        public float x;
        public float y;
        public float z;

        public Quaterion(){
            w = x = y = z = 0;
        }
        public Quaterion(float _w, float _x, float _y, float _z)
        {
            w = _w;
            x = _x;
            y = _y;
            z = _z;
        }

        public Quaterion EulerToQuaterion(float yaw, float pitch, float roll)
        {

            yaw *= Mathf.Deg2Rad;
            pitch *= Mathf.Deg2Rad;
            roll *= Mathf.Deg2Rad;
            float rollOver2 = roll * 0.5f;
            float sinRollOver2 = (float)Mathf.Sin(rollOver2);
            float cosRollOver2 = (float)Mathf.Cos(rollOver2);
            float pitchOver2 = pitch * 0.5f;
            float sinPitchOver2 = (float)Mathf.Sin(pitchOver2);
            float cosPitchOver2 = (float)Mathf.Cos(pitchOver2);
            float yawOver2 = yaw * 0.5f;
            float sinYawOver2 = (float)Mathf.Sin(yawOver2);
            float cosYawOver2 = (float)Mathf.Cos(yawOver2);
            Quaterion result = new Quaterion();
            result.w = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
            result.x = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
            result.y = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;
            result.z = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;

            return result;
        }
        public Vector3 QuaterionToEuler(Quaterion q)
        {
            //X rotation
            Vector3 eul = new Vector3();
            float sinX = 2.0f * (q.w * q.x + q.y * q.z);
            float cosX = 1.0f - 2.0f * (q.x * q.x + q.y * q.y);
            eul.y = Mathf.Atan2(sinX, cosX);

            //Y rotation
            float sinY = 2.0f * (q.w * q.y - q.z * q.x);
            eul.y = Mathf.Asin(sinY);

            //Z rotation
            float sinZ = 2.0f * (q.w * q.z + q.x * q.y);
            float cosZ = 1.0f - 2.0f * (q.y * q.y + q.z * q.z);
            eul.z = Mathf.Atan2(sinZ, cosZ);

            return eul;
        }
        public Vector3 QuaterionToEuler()
        {
            //X rotation
            Vector3 eul = new Vector3();
            float sinX = 2.0f * (w * x + y * z);
            float cosX = 1.0f - 2.0f * (x * x + y * y);
            eul.y = Mathf.Atan2(sinX, cosX);

            //Y rotation
            float sinY = 2.0f * (w * y - z * x);
            eul.y = Mathf.Asin(sinY);

            //Z rotation
            float sinZ = 2.0f * (w * z + x * y);
            float cosZ = 1.0f - 2.0f * (y * y + z * z);
            eul.z = Mathf.Atan2(sinZ, cosZ);

            return eul;
        }


        public string PrintOut()
        {
            return "(" + System.Math.Round((double)x,1) + ", " + System.Math.Round((double)y, 1) + ", " + System.Math.Round((double)z, 1) + ", " + System.Math.Round((double)w, 1) + ")";
        }
    }

    public class Matrix4x4
    {
        private float m00;
        private float m01;
        private float m02;
        private float m03;
        private float m10;
        private float m11;
        private float m12;
        private float m13;
        private float m20;
        private float m21;
        private float m22;
        private float m23;
        private float m30;
        private float m31;
        private float m32;
        private float m33;

        //Constructors
        public Matrix4x4()
        {
            m00 = m01 = m02 = m03 = m10 = m11 = m12 = m13 = m20 = m21 = m22 = m23 = m30 = m31 = m32 = m33 = 1;
        }
        public Matrix4x4(float n)
        {
            m00 = m01 = m02 = m03 = m10 = m11 = m12 = m13 = m20 = m21 = m22 = m23 = m30 = m31 = m32 = m33 = n;
        }
        public Matrix4x4(float _m00, float _m01, float _m02, float _m03,
                         float _m10, float _m11, float _m12, float _m13,
                         float _m20, float _m21, float _m22, float _m23,
                         float _m30, float _m31, float _m32, float _m33)
        {
            m00 = _m00;
            m01 = _m01;
            m02 = _m02;
            m03 = _m03;
            m10 = _m10;
            m11 = _m11;
            m12 = _m12;
            m13 = _m13;
            m20 = _m20;
            m21 = _m21;
            m22 = _m22;
            m23 = _m23;
            m30 = _m30;
            m31 = _m31;
            m32 = _m32;
            m33 = _m33;
        }
        public Matrix4x4(Matrix4x4 matrix)
        {
            m00 = matrix.m00;
            m01 = matrix.m01;
            m02 = matrix.m02;
            m03 = matrix.m03;
            m10 = matrix.m10;
            m11 = matrix.m11;
            m12 = matrix.m12;
            m13 = matrix.m13;
            m20 = matrix.m20;
            m21 = matrix.m21;
            m22 = matrix.m22;
            m23 = matrix.m23;
            m30 = matrix.m30;
            m31 = matrix.m31;
            m32 = matrix.m32;
            m33 = matrix.m33;
        }


        public Vector3 ExtractPosition()
        {
            Vector3 pos = new Vector3();
            pos.x = m03;
            pos.y = m13;
            pos.z = m23;
            return pos;
        }
        public Vector3 ExtractRotation()
        {

            Matrix4x4 temp = new Matrix4x4(this);
            Vector3 scaleExtract = temp.ExtractScale();
            Matrix4x4 temp1 = new Matrix4x4(temp.m00 / scaleExtract.x, temp.m01 / scaleExtract.y, temp.m02 / scaleExtract.z, temp.m03,
                                            temp.m10 / scaleExtract.x, temp.m11 / scaleExtract.y, temp.m12 / scaleExtract.z, temp.m13,
                                            temp.m20 / scaleExtract.x, temp.m21 / scaleExtract.y, temp.m22 / scaleExtract.z, temp.m23,
                                            temp.m30 / scaleExtract.x, temp.m31 / scaleExtract.y, temp.m32 / scaleExtract.z, temp.m33);


            Quaterion q = new Quaterion();
            q.w = Mathf.Sqrt(1.0f + temp1.m00 + temp1.m11 + temp1.m22) / 2.0f;
            q.w *= 4.0f;
            q.x = (temp1.m21 - temp1.m12) / q.w;
            q.y = (temp1.m02 - temp1.m20) / q.w;
            q.z = (temp1.m10 - temp1.m01) / q.w;


            return q.QuaterionToEuler();
        }
        public Vector3 ExtractScale()
        {
            Vector3 scale = new Vector3();
            scale.x = Mathf.Sqrt((m00 * m00) + (m10 * m10) + (m20 * m20) + (m30 * m30));
            scale.y = Mathf.Sqrt((m01 * m01) + (m11 * m11) + (m21 * m21) + (m31 * m31));
            scale.z = Mathf.Sqrt((m02 * m02) + (m12 * m12) + (m22 * m22) + (m32 * m32));
            return scale;
        }


        public void Translate(Vector3 transform)
        {
            m03 = transform.x;
            m13 = transform.y;
            m23 = transform.z;
        }
        public void Rotate(Vector3 rotation)
        {
            float cosAngle;
            float sinAngle;
            //Rotate on X
            Matrix4x4 tempX = new Matrix4x4();
            cosAngle = Mathf.Cos(rotation.x);
            sinAngle = Mathf.Sin(rotation.x);
            tempX.m11 = cosAngle;
            tempX.m12 = sinAngle;
            tempX.m21 = -sinAngle;
            tempX.m22 = cosAngle;
            Multiply(tempX);

            //Rotate on Y
            Matrix4x4 tempY = new Matrix4x4();
            cosAngle = Mathf.Cos(rotation.y);
            sinAngle = Mathf.Sin(rotation.y);
            tempY.m00 = cosAngle;
            tempY.m02 = -sinAngle;
            tempY.m20 = sinAngle;
            tempY.m22 = cosAngle;
            this.Multiply(tempY);

            //Rotate on Z
            Matrix4x4 tempZ = new Matrix4x4();
            cosAngle = Mathf.Cos(rotation.z);
            sinAngle = Mathf.Sin(rotation.z);
            tempZ.m00 = cosAngle;
            tempZ.m01 = sinAngle;
            tempZ.m10 = -sinAngle;
            tempZ.m11 = cosAngle;
            Multiply(tempZ);
        }
        public void Scale(Vector3 scale)
        {
            Matrix4x4 temp = new Matrix4x4();
            temp.m00 = scale.x;
            temp.m11 = scale.y;
            temp.m22 = scale.z;
            Multiply(temp);
        }

        public void Multiply(Matrix4x4 matrix)
        {
            Matrix4x4 product = new Matrix4x4(this);

            m00 = (product.m00 * matrix.m00) + (product.m01 * matrix.m10) + (product.m02 * matrix.m20) + (product.m03 * matrix.m30);
            m01 = (product.m00 * matrix.m01) + (product.m01 * matrix.m11) + (product.m02 * matrix.m21) + (product.m03 * matrix.m31);
            m02 = (product.m00 * matrix.m02) + (product.m01 * matrix.m12) + (product.m02 * matrix.m22) + (product.m03 * matrix.m32);
            m03 = (product.m00 * matrix.m03) + (product.m01 * matrix.m13) + (product.m02 * matrix.m23) + (product.m03 * matrix.m33);
            m10 = (product.m10 * matrix.m00) + (product.m11 * matrix.m10) + (product.m12 * matrix.m20) + (product.m13 * matrix.m30);
            m11 = (product.m10 * matrix.m01) + (product.m11 * matrix.m11) + (product.m12 * matrix.m21) + (product.m13 * matrix.m31);
            m12 = (product.m10 * matrix.m02) + (product.m11 * matrix.m12) + (product.m12 * matrix.m22) + (product.m13 * matrix.m32);
            m13 = (product.m10 * matrix.m03) + (product.m11 * matrix.m13) + (product.m12 * matrix.m23) + (product.m13 * matrix.m33);
            m20 = (product.m20 * matrix.m00) + (product.m21 * matrix.m10) + (product.m22 * matrix.m20) + (product.m23 * matrix.m30);
            m21 = (product.m20 * matrix.m01) + (product.m21 * matrix.m11) + (product.m22 * matrix.m21) + (product.m23 * matrix.m31);
            m22 = (product.m20 * matrix.m02) + (product.m21 * matrix.m13) + (product.m22 * matrix.m22) + (product.m23 * matrix.m32);
            m23 = (product.m20 * matrix.m03) + (product.m21 * matrix.m13) + (product.m22 * matrix.m23) + (product.m23 * matrix.m33);
            m30 = (product.m30 * matrix.m00) + (product.m31 * matrix.m10) + (product.m32 * matrix.m20) + (product.m33 * matrix.m30);
            m31 = (product.m30 * matrix.m01) + (product.m31 * matrix.m11) + (product.m32 * matrix.m21) + (product.m33 * matrix.m31);
            m32 = (product.m30 * matrix.m02) + (product.m31 * matrix.m12) + (product.m32 * matrix.m22) + (product.m33 * matrix.m32);
            m33 = (product.m30 * matrix.m03) + (product.m31 * matrix.m13) + (product.m32 * matrix.m23) + (product.m33 * matrix.m33);


        }
        public Matrix4x4 Multiply(Matrix4x4 matrix1, Matrix4x4 matrix2)
        {
            Matrix4x4 product = new Matrix4x4();

            product.m00 = (matrix1.m00 * matrix2.m00) + (matrix1.m01 * matrix2.m10) + (matrix1.m02 * matrix2.m20) + (matrix1.m03 * matrix2.m30);
            product.m01 = (matrix1.m00 * matrix2.m01) + (matrix1.m01 * matrix2.m11) + (matrix1.m02 * matrix2.m21) + (matrix1.m03 * matrix2.m31);
            product.m02 = (matrix1.m00 * matrix2.m02) + (matrix1.m01 * matrix2.m12) + (matrix1.m02 * matrix2.m22) + (matrix1.m03 * matrix2.m32);
            product.m03 = (matrix1.m00 * matrix2.m03) + (matrix1.m01 * matrix2.m13) + (matrix1.m02 * matrix2.m23) + (matrix1.m03 * matrix2.m33);
            product.m10 = (matrix1.m10 * matrix2.m00) + (matrix1.m11 * matrix2.m10) + (matrix1.m12 * matrix2.m20) + (matrix1.m13 * matrix2.m30);
            product.m11 = (matrix1.m10 * matrix2.m01) + (matrix1.m11 * matrix2.m11) + (matrix1.m12 * matrix2.m21) + (matrix1.m13 * matrix2.m31);
            product.m12 = (matrix1.m10 * matrix2.m02) + (matrix1.m11 * matrix2.m12) + (matrix1.m12 * matrix2.m22) + (matrix1.m13 * matrix2.m32);
            product.m13 = (matrix1.m10 * matrix2.m03) + (matrix1.m11 * matrix2.m13) + (matrix1.m12 * matrix2.m23) + (matrix1.m13 * matrix2.m33);
            product.m20 = (matrix1.m20 * matrix2.m00) + (matrix1.m21 * matrix2.m10) + (matrix1.m22 * matrix2.m20) + (matrix1.m23 * matrix2.m30);
            product.m21 = (matrix1.m20 * matrix2.m01) + (matrix1.m21 * matrix2.m11) + (matrix1.m22 * matrix2.m21) + (matrix1.m23 * matrix2.m31);
            product.m22 = (matrix1.m20 * matrix2.m02) + (matrix1.m21 * matrix2.m13) + (matrix1.m22 * matrix2.m22) + (matrix1.m23 * matrix2.m32);
            product.m23 = (matrix1.m20 * matrix2.m03) + (matrix1.m21 * matrix2.m13) + (matrix1.m22 * matrix2.m23) + (matrix1.m23 * matrix2.m33);
            product.m30 = (matrix1.m30 * matrix2.m00) + (matrix1.m31 * matrix2.m10) + (matrix1.m32 * matrix2.m20) + (matrix1.m33 * matrix2.m30);
            product.m31 = (matrix1.m30 * matrix2.m01) + (matrix1.m31 * matrix2.m11) + (matrix1.m32 * matrix2.m21) + (matrix1.m33 * matrix2.m31);
            product.m32 = (matrix1.m30 * matrix2.m02) + (matrix1.m31 * matrix2.m12) + (matrix1.m32 * matrix2.m22) + (matrix1.m33 * matrix2.m32);
            product.m33 = (matrix1.m30 * matrix2.m03) + (matrix1.m31 * matrix2.m13) + (matrix1.m32 * matrix2.m23) + (matrix1.m33 * matrix2.m33);

            return product;
        }

        public Matrix4x4 Transpose()
        {
            Matrix4x4 temp = new Matrix4x4(this);
            temp.m00 = this.m00;
            temp.m01 = this.m10;
            temp.m02 = this.m20;
            temp.m03 = this.m30;
            temp.m10 = this.m01;
            temp.m11 = this.m11;
            temp.m12 = this.m21;
            temp.m13 = this.m31;
            temp.m20 = this.m02;
            temp.m21 = this.m12;
            temp.m22 = this.m22;
            temp.m23 = this.m32;
            temp.m30 = this.m03;
            temp.m31 = this.m13;
            temp.m32 = this.m23;
            temp.m33 = this.m33;
            return temp;
        }

        public string PrintOut()
        {
            return m00 + " " + m01 + " " + m02 + " " + m03 + "\n" +
                   m10 + " " + m11 + " " + m12 + " " + m13 + "\n" +
                   m20 + " " + m21 + " " + m22 + " " + m23 + "\n" +
                   m30 + " " + m31 + " " + m32 + " " + m33 + "\n";
        }
    }

    public class Transform
    {
        private Vector3 localPosition;
        private Vector3 localRotation;
        private Vector3 localScale;
        private Transform parent;
        private Transform[] children;
        private int childrenCount = 0;
        public Transform()
        {
        }
        public Transform(Transform trans)
        {
            localPosition = new Vector3(trans.localPosition);
            localRotation = new Vector3(trans.localRotation);
            localScale = new Vector3(trans.localScale);
            parent = trans.parent;
            children = trans.children;
        }
        public void SetLocalPosition(Vector3 pos)
        {
            localPosition = new Vector3();
            localPosition.x = pos.x;
            localPosition.y = pos.y;
            localPosition.z = pos.z;
        }
        public void AddChild(Transform child)
        {
            Transform[] temp = new Transform[childrenCount];
            children = new Transform[childrenCount + 1];
            children[childrenCount] = new Transform(child);
            child.parent = this;
        }



        public Vector3 GetWorldPosition()
        {
            if(parent == null)
                return localPosition;
            else
            {
                return parent.GetWorldPosition() + localPosition;
            }
        }

        public Vector3 GetWorldRotation()
        {
            if(parent == null)
                return localRotation.ReturnVectorProduct(new Vector3(0, 1f, 0));
            else
            {
                return parent.GetWorldRotation() + localRotation.ReturnVectorProduct(new Vector3(0, 1f, 0));
            }
        }
        public Vector3 GetWorldScale()
        {
            if(parent == null)
                return localScale;
            else
            {
                return parent.GetWorldScale() + localScale;
            }
        }

        public Matrix4x4 GetObjectToWorldMatrix()
        {
            Matrix4x4 temp = new Matrix4x4();
            return temp;
        }

    }

    public class TransformCollection
    {
        public Transform t1;
        public Transform t2;
        public Transform t3;
        public Transform t4;
        public Transform t5;
        public Transform t6;
        public Transform t7;
    }
}

