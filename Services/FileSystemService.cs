using System;
using System.IO;
using Ookii.Dialogs.Wpf;

namespace StarBase1.Services;

public interface IFileService : IDisposable
{
    IFileService SetInitialDirectory(DirectoryInfo directory);
    FileInfo OpenFile();
    DirectoryInfo OpenFolder();
    string ReadFile(FileInfo file);
    void WriteFile(FileInfo file, string content);
    void MoveFile(FileInfo source, FileInfo target);
    void CopyFile(FileInfo source, FileInfo target);
    void DeleteFile(FileInfo file);
    void MoveDirectory(DirectoryInfo source, DirectoryInfo target);
    void CopyDirectory(DirectoryInfo source, DirectoryInfo target);
    void DeleteDirectory(DirectoryInfo directory);
}

public class FileService : IFileService
{
    private VistaOpenFileDialog _fileDialog;
    private VistaFolderBrowserDialog _folderDialog;

    public FileService()
    {
        _fileDialog = new VistaOpenFileDialog();
        _folderDialog = new VistaFolderBrowserDialog();
    }

    public IFileService SetInitialDirectory(DirectoryInfo directory)
    {
        _fileDialog.InitialDirectory = directory.FullName;
        _folderDialog.SelectedPath = directory.FullName;
        return this;
    }

    public FileInfo OpenFile()
    {
        if (_fileDialog.ShowDialog() == true)
        {
            return new FileInfo(_fileDialog.FileName);
        }
        return null;
    }

    public DirectoryInfo OpenFolder()
    {
        if (_folderDialog.ShowDialog() == true)
        {
            return new DirectoryInfo(_folderDialog.SelectedPath);
        }
        return null;
    }

    public string ReadFile(FileInfo file)
    {
        using (StreamReader reader = file.OpenText())
        {
            return reader.ReadToEnd();
        }
    }

    public void WriteFile(FileInfo file, string content)
    {
        using (StreamWriter writer = file.CreateText())
        {
            writer.Write(content);
        }
    }

    public void MoveFile(FileInfo source, FileInfo target)
    {
        source.MoveTo(target.FullName);
    }

    public void CopyFile(FileInfo source, FileInfo target)
    {
        source.CopyTo(target.FullName);
    }

    public void DeleteFile(FileInfo file)
    {
        file.Delete();
    }

    public void MoveDirectory(DirectoryInfo source, DirectoryInfo target)
    {
        source.MoveTo(target.FullName);
    }

    public void CopyDirectory(DirectoryInfo source, DirectoryInfo target)
    {
        foreach (FileInfo file in source.GetFiles())
        {
            file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
        foreach (DirectoryInfo dir in source.GetDirectories())
        {
            DirectoryInfo targetDir = target.CreateSubdirectory(dir.Name);
            CopyDirectory(dir, targetDir);
        }
    }

    public void DeleteDirectory(DirectoryInfo directory)
    {
        directory.Delete(true);
    }

    public void Dispose()
    {
        // _fileDialog?.Dispose();
        // _folderDialog?.Dispose();
    }
}
