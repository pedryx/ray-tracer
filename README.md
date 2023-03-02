# Ray Tracer
Simple command line ray tracer.

Creates view into the scene.

## non-mandatory names arguments:
- config - specify path to non-default config file

## config file
Default config file is config.xml. It is XML file where you can specify
how output file should look like.

All settings has to be contained in Config node:

    <Config>
    ...
    </Config>

You can specify name of the output file:

    <OutputFile>demo.pfm</OutputFile>

You can specify camera settings:

	<Camera>
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

You can specify scene settings which will be specified in following block:

    <Scene>
    ...
    </Scene>

### Scene settings

You can set color of the background:

    <BackgroundColor>
        <R>0.1</R>
        <G>0.2</G>
        <B>0.3</B>
    </BackgroundColor>

You can specify light sources, they can be either ambient or point:

    <LightSources>
        <!--Ambient light source-->
        <Ambient>
            <Intensity>
                <X>1</X>
                <Y>1</Y>
                <Z>1</Z>
            </Intensity>
        </Ambient>
        <!--Point light source.-->
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
        ...
    </LightSources>

You can specify materials:

    <Materials>
        <Material>
            <!--Name of the material.-->
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
        ...
    </Materials>

You can specify shapes, only spheres and planes are currently supported:

    <Shapes>
        <Sphere>
            <!--Name of shape's material.-->
            <Material>yellow</Material>
            <Position>
                <X>0</X>
                <Y>0</Y>
                <Z>0</Z>
            </Position>
            <Radius>1</Radius>
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
    ...
    </Shapes>

## example usage:

    ./RayTracer.exe --config: my_config.xml