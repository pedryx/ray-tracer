# Ray Tracer
Simple command line ray tracer.

Creates simple image.

## non-mandatory names arguments:
- config - specify path to non-default config file

## config file
Default config file is config.xml. It is XML file where you can specify
following settings:
- ImageWidth - width of the image
- ImageHeight - height of the image
- OutputFile - file where generated image should be placed
- SinCoeficient - coeficient of sinus function, used for rendering simple image

## examples:
example usage:

    ./RayTracer.exe --config: my_config.xml
    
example config file:

    <?xml version="1.0" encoding="utf-8" ?>
    <Config>
        <ImageWidth>400</ImageWidth>
        <ImageHeight>300</ImageHeight>
        <OutputFile>my_image.pfm</OutputFile>
        <SinCoeficient>0.3</SinCoeficient>
    </Config>