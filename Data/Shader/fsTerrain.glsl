#version 330 core


uniform vec3 lightPos;
uniform vec3 viewPos;
uniform vec3 lightCol;
uniform vec3 lightSpec;
uniform float lightDepth; 
uniform float lightRange;
uniform vec3 ambCE;
uniform vec3 matSpec;
uniform float matS;
uniform vec3 matDiff;
// Interpolated values from the vertex shaders

in vec3 fragPos;
in vec2 texCoords;
in vec3 TLP;
in vec3 TVP;
in vec3 TFP;

// Ouput data
out vec3 colorout;

// Values that stay constant for the whole mesh.
uniform sampler2D tC;
uniform sampler2D tN;
uniform samplerCube tS;
uniform sampler2D tSpec;


    vec3 sampleOffsetDirections[20] = vec3[]
(
   vec3( 1,  1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1,  1,  1), 
   vec3( 1,  1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1,  1, -1),
   vec3( 1,  1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1,  1,  0),
   vec3( 1,  0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1,  0, -1),
   vec3( 0,  1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0,  1, -1)
);   

void main(){
 
    vec3 normal = texture2D(tN,texCoords).rgb;
    normal = normalize(normal * 2.0 - 1.0);

    vec3 color = texture2D(tC,texCoords).rgb * matDiff;

    vec3 ambient = 0.1 * color;

    vec3 lightDir = normalize(TLP - TFP);

    float diff = max(dot(lightDir,normal),0.0);

    vec3 diffuse = (diff * color * lightCol) + ambCE;


    vec3 viewDir = normalize(TVP-TFP);
    vec3 reflectDir = reflect(-lightDir,normal);
    vec3 halfwayDir = normalize(lightDir+viewDir);

    float spec = pow(max(dot(normal,halfwayDir),0.0),32.0);

    spec = spec * matS;

    vec3 specular = ((lightSpec + matSpec) * spec); 

    //Shadows


    float shadow = 0.0;
    float bias = 7.2f;
    int samples = 18;
    float viewDistance = length(viewPos - fragPos);
    float diskRadius = 0.006f;
    vec3 fragToLight = fragPos - lightPos;
    float currentDepth = length(fragToLight);
    float ld2 = currentDepth/lightRange;
    if(ld2>1.0) ld2 = 1.0;
    ld2 = 1.0 - ld2;
    fragToLight = normalize(fragToLight);
    for(int i=0;i<samples;i++){

        float closestDepth = texture(tS,fragToLight + sampleOffsetDirections[i] * diskRadius).r;
        closestDepth *= lightDepth;
        if((currentDepth - bias) > closestDepth){
            shadow += 1.0;
        }

    }
    shadow /= float(samples);
    shadow = 1.0 - shadow;
    colorout = (color * diffuse) * vec3(shadow,shadow,shadow) + (specular * texture2D(tSpec,texCoords).rgb);

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
