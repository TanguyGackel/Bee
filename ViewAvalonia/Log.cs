namespace MS_Lib;

[Flags]
public enum LogLevel
{
    Info,
    Warning,
    Error
}

public static class Log
{
    public static string dir_path;

    private static StreamWriter? default_path_info;

    private static StreamWriter? default_path_error;

    public static string Info
    {
        set
        {
            if (IsWriterOpen(default_path_info))
            {
                default_path_info.Close();
                
            }
            default_path_info = new StreamWriter(value, true);
        }
    }
    public static string Error
    {
        set
        {
            if (IsWriterOpen(default_path_error))
            {
                default_path_error.Close();
                
            }
            default_path_error = new StreamWriter(value, true);
        }
    }


    public static void WriteLog(LogLevel level, string message, string filepath)
    {
        
        try
        {
            string fullPath = Path.Combine(dir_path, filepath);
            using (StreamWriter outputFile = new StreamWriter(fullPath, true))
            {
                string formattedMessage = FormatMessage(level, message);
                outputFile.WriteLine(formattedMessage);
            }
        }
        catch (Exception ex)
        {
            return; //TODO
        }

        
    }

    public static void WriteLog(LogLevel level, string message)
    {

        try
        {
            switch (level)
            {
                case LogLevel.Info | LogLevel.Warning:
                    if (default_path_info == null)
                        throw new NullReferenceException("Info log path is null");
                    default_path_info.WriteLine(FormatMessage(level, message));
                    break;
                
                case LogLevel.Error:
                    if (default_path_error == null)
                        throw new NullReferenceException("Info log path is null");
                    default_path_error.WriteLine(FormatMessage(level, message));
                    break;
            }
        }
        catch (Exception ex)
        {
            return;
        }

    }
    
    private static string FormatMessage(LogLevel level, string message)
    {
        DateTime now = DateTime.Now;
        string timestamp = $"{now:yyyy/MM/dd-HH:mm:ss}:{now.Millisecond}:{now.Microsecond}:{now.Nanosecond}";
        return $"{timestamp} [{level}] : {message}";
    }
    
    private static bool IsWriterOpen(StreamWriter? writer)
    {
        return writer != null && writer.BaseStream != null && writer.BaseStream.CanWrite;
    }
    

}

