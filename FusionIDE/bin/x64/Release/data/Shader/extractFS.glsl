#version 330 core

in vec2 UV;

out vec3 color;

uniform sampler2D tR;
uniform float MinLevel;


void main(){
vec3 fc = texture2D(tR,UV).rgb;

    float tv = fc.r+fc.g+fc.b;
    tv = tv / 3.0f;
    if(tv<MinLevel){
        fc = vec3(0,0,0);
    }

    color = fc;
}