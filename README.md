# Ray Tracer
Simple command line ray tracer.

Creates empty image.

## non-mandatory names arguments:
- config - specify path to non-default config file

## config file
Default config file is config.xml. It is XML file with where you has to
specify following settings (not setting them is undefined behaviour):
- ImageWidth - width of the image
- ImageHeight - height of the image
- OutputFile - file where generated image should be placed

## examples:
example usage:
    ./RayTracer.exe --config: my_config.xml
example config file:
    <?xml version="1.0" encoding="utf-8" ?>
    <Config>
        <ImageWidth>800</ImageWidth>
        <ImageHeight>600</ImageHeight>
        <OutputFile>my_image.pfm</OutputFile>
    </Config>