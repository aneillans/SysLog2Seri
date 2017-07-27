# SysLog2Seri

This project lets you receive UDP SysLog messages (on port 514 - standard SysLog), and forward it to the Seri logging framework for processing - which can then save it to a rolling log file, print to console or forward to Seq.

Configuration is via the .config file; simply add in the Seri logging entries you need (note that if you wish to use any other targets you will need to recompile).

## Log to Console:

```xml
<configuration>
  <appSettings>
    <add key="serilog:using:Console" value="Serilog.Sinks.Console" />
    <add key="serilog:write-to:Console" />
```

## Log to Rolling File:

```xml
<configuration>
  <appSettings>
    <add key="serilog:using:RollingFile" value="Serilog.Sinks.RollingFile" />
    <add key="serilog:write-to:RollingFile.pathFormat" value="log-{Date}.txt" />
```

The parameters that can be set through the `serilog:write-to:RollingFile` keys are the method parameters accepted by the `WriteTo.RollingFile()` configuration method. This means, for example, that the `fileSizeLimitBytes` parameter can be set with:

```xml
    <add key="serilog:write-to:RollingFile.fileSizeLimitBytes" value="1234567" />
```

## Log to SEQ:

```xml
<configuration>
  <appSettings>
    <add key="serilog:using:Seq" value="Serilog.Sinks.Seq" />
    <add key="serilog:write-to:Seq.serverUrl" value="http://localhost:5341" />
    <add key="serilog:write-to:Seq.apiKey" value="[optional API key here]" />
```


## Running

Run SysLog2Seri.exe after updating the .config file; the application will run as a Console Application, or alternatively you can run SysLog2Seri install to install as a System Service. (SysLog2Seri uninstall will remove it). - Thanks to TopShelf :)