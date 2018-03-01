using System.Linq;
using UnityEngine;

//Right now it does not generate a "smooth" plane
//The plane is rotated towards -Z and Z = 0
class PlaneGenerator : IMeshGenerator
{
    int horizontal, vertical;

    Vector2 diagonal;
    Vector3 addPos;
    Vector2 addUv;

    public PlaneGenerator(Vector2 lowerLeft, Vector2 upperRight, int horizontal, int vertical)
    {
        this.horizontal = horizontal;
        this.vertical = vertical;

        this.diagonal = upperRight - lowerLeft;
        this.addPos = new Vector3(diagonal.x / horizontal, diagonal.y / vertical, 0.0f);
        this.addUv  = new Vector2(1.0f / horizontal, 1.0f / vertical);
    }


    public MeshAttributes Generate()
    {
        int numVerts = horizontal * vertical * 6;

        MeshAttributes mesh = new MeshAttributes
        {
            vertices = new Vector3[numVerts],
            uv = new Vector2[numVerts],
            triangles = Enumerable.Range(0, numVerts).ToArray<int>(),
            normals = new Vector3[] { Vector3.back },
            normalIndex = Enumerable.Repeat<int>(0, numVerts).ToArray<int>(),
            tangent = new Vector3[] { Vector3.up },
            tangentIndex = Enumerable.Repeat<int>(0, numVerts).ToArray<int>()
        };


        for (int y=0; y<vertical; ++y)
        {
            for (int x=0; x<horizontal; ++x)
            {
                int index = (y * horizontal + x) * 6;
                GenerateQuad(mesh, x, y, index);
                GenerateUv(mesh, x, y, index);
            }
        }

        return mesh;
    }

    private void GenerateQuad(MeshAttributes mesh, int x, int y, int index)
    {

        Vector3 position = new Vector3(addPos.x * x, addPos.y * y, 0.0f);
        Vector3 nextX = position + addPos.x * Vector3.right;
        Vector3 nextY = position + addPos.y * Vector3.up;
        Vector3 nextXY = position + addPos;


        //Bottom triangle
        mesh.vertices[index] = position;
        mesh.vertices[index + 1] = nextY;
        mesh.vertices[index + 2] = nextX;

        //Top triangle
        mesh.vertices[index + 3] = nextXY;
        mesh.vertices[index + 4] = nextX;
        mesh.vertices[index + 5] = nextY;
    }

    private void GenerateUv(MeshAttributes mesh, int x, int y, int index)
    {
        Vector2 texCoord = new Vector2(addUv.x * x, addUv.y * y);
        Vector2 nextU = texCoord + addUv.x * Vector2.right;
        Vector2 nextV = texCoord + addUv.y * Vector2.up;
        Vector2 nextUV = texCoord + addUv;

        //Bottom triangle
        mesh.uv[index] = texCoord;
        mesh.uv[index + 1] = nextV;
        mesh.uv[index + 2] = nextU;

        //Top triangle
        mesh.uv[index + 3] = nextUV;
        mesh.uv[index + 4] = nextU;
        mesh.uv[index + 5] = nextV;
    }
}

