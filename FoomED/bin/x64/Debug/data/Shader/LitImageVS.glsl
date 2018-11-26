


void main()
{

    gl_TexCoord[0] = gl_MultiTexCoord0;
    // output the transformed vertex
    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}