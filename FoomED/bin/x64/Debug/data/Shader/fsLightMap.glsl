#version 330 core



// Interpolated values from the vertex shaders


in vec2 texCoords;


// Ouput data
out vec3 colorout;

// Values that stay constant for the whole mesh.
uniform sampler2D tC;
 

void main(){
 
  
    colorout = texture2D(tC,texCoords).rgb;   //;(color * diffuse) * vec3(shadow,shadow,shadow) + (specular * texture2D(tSpec,texCoords).rgb);

    /*
    vec3 ld = (fragPos-lightPos);

    float closeD = texture(tS,ld).r;

    closeD *= lightDepth;

    float curD = length(ld);


    float shade = 0.0;

    curD = curD - 2.0f;

    if(curD<closeD){
        shade = 1.0;
    }
    else{
        shade =  0.0;
    }


colorout =((diffuse) * vec3(shade,shade,shade))+specular;
*/
return;
 

    //colorout = textureCube(tS,normal).rgb;  //(color*diff*lightCol)+specular;




 
}
