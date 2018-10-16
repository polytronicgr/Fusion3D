#version 440 core

// Input vertex data, different for all executions of this shader.
layout(location = 0) in vec3 vP;

// Output data ; will be interpolated for each fragment.
out vec2 UV;

void main(){
	gl_Position =  vec4(vP,1);
	UV = (vP.xy+vec2(1,1))/2.0;
}



