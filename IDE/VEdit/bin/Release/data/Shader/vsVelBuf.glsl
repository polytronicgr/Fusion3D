#version 440 core

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

uniform mat4 pview;
uniform mat4 pmodel;





// Output data ; will be interpolated for each fragment.
out vec3 fragPos;
out vec2 texCoords;
out vec2 speed;

vec2 getSpeed()
{
   vec4 oldScreenCoord = proj *  pview *  pmodel * vec4(aPos,1.0);
  vec4 newScreenCoord = proj * view * model * vec4(aPos,1.0);
  //vec2 v = (newScreenCoord.xy / newScreenCoord.w);
   //vec2 v2 = (oldScreenCoord.xy / oldScreenCoord.w);
    float x1 = newScreenCoord.x;// / newScreenCoord.w;
	float y1 = newScreenCoord.y; /// newScreenCoord.w;

	float x2 = oldScreenCoord.x;// / oldScreenCoord.w;
	float y2 = oldScreenCoord.y; /// oldScreenCoord.w;

	vec2 v = vec2(x1-x2,y1-y2);
    v.x = v.x * 0.1;
	v.y = v.y * 0.1;
	return v;
}

void main(){

	fragPos = vec3(model * vec4(aPos,1.0));
	texCoords = aTexCoords;

	vec2 spd = getSpeed();

	

	speed = spd;

	gl_Position = proj * view * model* vec4(aPos,1.0);

	
}

