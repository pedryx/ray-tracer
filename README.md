# Ray Tracer
Simple command line ray tracer.

Creates view into the scene.

## non-mandatory names arguments:
- config - specify path to non-default config file
- graph - specify path to non-default file with graph scene hierarchy

## config file
Default config file is config.xml. It is XML file where you can specify
how output file should look like.

All settings has to be contained in Config node:

    <Config>
    ...
    </Config>

You can specify name of the output file:

    <OutputFile>demo.pfm</OutputFile>

You can configure various settings:

    <Shadows>true</Shadows>
	<Reflections>true</Reflections>
	<Refractions>true</Refractions>
	<MaxDepth>5</MaxDepth>
	<SamplesPerPixel>16</SamplesPerPixel>

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
            <!--How much light is reflected.-->
            <Reflection>0.0</Reflection>
            <!--How much light is refracted.-->
			<Refraction>0.5</Refraction>
			<RefractiveIndex>1.47</RefractiveIndex>
        </Material>
        ...
    </Materials>

## graph file
Default graph file is scene.xml. It is XML file where you can specify
composition of scene elements.

It uses tree structure. Whole tree has to be wrapper in root node called
InnerNode:

    <InnerNode>
    ...
    </InnerNode>

There are two types of nodes inner and leaf. Root inner node is called InnerNode
other inner nodes are caled just Node. In (Inner)Node you can specify following
settings:

    <Node>
        <Transformations>
        ...
        </Transformation>
        <Material>Red</Material>
        <Nodes>
        ...
        </Nodes>
    </Node>

In Transformation section you can specify transformations which will be applied
to whole subtree. Transformation are applied in direction to the root, and from
last to first. Following transformations are supported:

    <Transformations>
    	<Transform>Translate 2 0 10</Transform>
        <Transform>Scale 0.55</Transform>
        <Transform>RotateX -0.3</Transform>
        <Transform>RotateY -0.7</Transform>
        <Transform>RotateZ -0.4</Transform>
    </Transformations>

The Nodes section can contains any number of nodes (either Leaf or Node). In
Leaf node you can specify following settings:

    <Leaf>
        <Shapes>
        ...
        </Shapes>
    </Leaf>

In the Shapes section, you can specify shapes, only spheres and planes are
currently supported:

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

    ./RayTracer.exe --config: my_config.xml --graph: my_scene.xml