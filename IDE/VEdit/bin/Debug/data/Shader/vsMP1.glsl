#version 330 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoords;
layout(location = 2) in vec3 aNormal;
layout(location = 3) in vec3 aBiTangent;
layout(location = 4) in vec3 aTangent;


// Values that stay constant for the whole mesh.
uniform mat4 view;
uniform mat4 model;
uniform mat4 proj;

uniform vec3 lightPos;
uniform vec3 viewPos;


// Output data ; will be interpolated for each fragment.
out vec3 fragPos;
out vec2 texCoords;
out vec3 TLP;
out vec3 TVP;
out vec3 TFP;
out vec3 rPos;
void main(){

	fragPos = vec3(model * vec4(aPos,1.0));
	texCoords = aTexCoords;

	mat3 normalMatrix = transpose(inverse(mat3(model)));

    vec3 T = normalize(normalMatrix * aTangent);
	vec3 N = normalize(normalMatrix * aNormal);
	
	T = normalize(T-dot(T,N) *N);
	
	vec3 B = cross(N,T);

	mat3 TBN = transpose(mat3(T,B,N));

	TLP = TBN * lightPos;
	TVP = TBN * viewPos;
	TFP = TBN * fragPos;
	


	gl_Position = proj * view * model* vec4(aPos,1.0);

	
}

