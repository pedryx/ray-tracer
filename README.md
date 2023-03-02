# Ray Tracer
Simple command line ray tracer.

Creates view into the scene.

## non-mandatory names arguments:
- config - specify path to non-default config file

## config file
Default config file is config.xml. It is XML file where you can specify
how output file should look like.

## examples:

### example usage:

    ./RayTracer.exe --config: my_config.xml

### example config file:

    <?xml version="1.0" encoding="utf-8" ?>
    <Config>
        <OutputFile>demo.pfm</OutputFile>

        <Camera>
            <!--Size of the output image.-->
            <Resolution>
                <X>600</X>
                <Y>450</Y>
            </Resolution>
            <Position>
                <X>0.6</X>
                <Y>0.0</Y>
                <Z>-5.6</Z>
            </Position>
            <Direction>
                <X>0.0</X>
                <Y>-0.03</Y>
                <Z>1.0</Z>
            </Direction>
            <!--Field of view.-->
            <FOV>40</FOV>
        </Camera>
        
        <Scene>
            <BackgroundColor>
                <R>0.1</R>
                <G>0.2</G>
                <B>0.3</B>
            </BackgroundColor>

            <!--Collection of light sources.-->
            <LightSources>
                <Ambient>
                    <Intensity>
                        <X>1</X>
                        <Y>1</Y>
                        <Z>1</Z>
                    </Intensity>
                </Ambient>
                <Point>
                    <Intensity>
                        <X>1</X>
                        <Y>1</Y>
                        <Z>1</Z>
                    </Intensity>
                    <Position>
                        <X>-10</X>
                        <Y>8</Y>
                        <Z>-6</Z>
                    </Position>
                </Point>
                <Point>
                    <Intensity>
                        <X>0.3</X>
                        <Y>0.3</Y>
                        <Z>0.3</Z>
                    </Intensity>
                    <Position>
                        <X>0</X>
                        <Y>20</Y>
                        <Z>-3</Z>
                    </Position>
                </Point>
            </LightSources>
            
            <!--Collection of materials.-->
            <Materials>
                <Material>
                    <Name>yellow</Name>
                    <!--Ambient coefficient.-->
                    <Ambient>0.1</Ambient>
                    <!--Diffuse coefficient.-->
                    <Diffuse>0.8</Diffuse>
                    <!--Specular coefficient.-->
                    <Specular>0.2</Specular>
                    <Highlight>10</Highlight>
                    <Color>
                        <R>1.0</R>
                        <G>1.0</G>
                        <B>0.2</B>
                    </Color>
                </Material>
                <Material>
                    <Name>blue</Name>
                    <Ambient>0.1</Ambient>
                    <Diffuse>0.5</Diffuse>
                    <Specular>0.5</Specular>
                    <Highlight>150</Highlight>
                    <Color>
                        <R>0.2</R>
                        <G>0.3</G>
                        <B>1.0</B>
                    </Color>
                </Material>
                <Material>
                    <Name>red</Name>
                    <Ambient>0.1</Ambient>
                    <Diffuse>0.6</Diffuse>
                    <Specular>0.4</Specular>
                    <Highlight>80</Highlight>
                    <Color>
                        <R>0.8</R>
                        <G>0.2</G>
                        <B>0.2</B>
                    </Color>
                </Material>
            </Materials>

            <!--Collection of scene objects.-->
            <Shapes>
                <Sphere>
                    <!--Name of material of the object.-->
                    <Material>yellow</Material>
                    <Position>
                        <X>0</X>
                        <Y>0</Y>
                        <Z>0</Z>
                    </Position>
                    <Radius>1</Radius>
                </Sphere>
                <Sphere>
                    <Material>blue</Material>
                    <Position>
                        <X>1.4</X>
                        <Y>-0.7</Y>
                        <Z>-0.5</Z>
                    </Position>
                    <Radius>0.6</Radius>
                </Sphere>
                <Sphere>
                    <Material>red</Material>
                    <Position>
                        <X>-0.7</X>
                        <Y>0.7</Y>
                        <Z>-0.8</Z>
                    </Position>
                    <Radius>0.1</Radius>
                </Sphere>
                <Plane>
                    <Material>red</Material>
                    <Position>
                        <X>0</X>
                        <Y>-1.5</Y>
                        <Z>0</Z>
                    </Position>
                    <Normal>
                        <X>0</X>
                        <Y>1</Y>
                        <Z>0</Z>
                    </Normal>
                </Plane>
            </Shapes>
            
        </Scene>
    </Config>