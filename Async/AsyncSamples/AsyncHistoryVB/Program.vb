Imports System
Imports System.IO
Imports System.Net
Imports System.Threading.Tasks

Module Program
    Private Const url As String = "http://www.cninnovation.com"
    Sub Main(args As String())
        SyncronizedAPI()
        AsynchronousPattern()
        EventBasedAsyncPattern()
        CallAsyncTaskBasedAsyncPatternAsync()
        Console.ReadLine()
    End Sub

    Private Async Sub CallAsyncTaskBasedAsyncPatternAsync()
        Await TaskBasedAsyncPatternAsync()
    End Sub

    Private Sub SyncronizedAPI()
        Console.WriteLine(NameOf(SyncronizedAPI))
        Using client As New WebClient()
            Dim content As String = client.DownloadString(url)
            Console.WriteLine(content.Substring(0, 100))
            Console.WriteLine()
        End Using
    End Sub

    Private Async Function TaskBasedAsyncPatternAsync() As Task(Of String)
        Console.WriteLine("TaskBasedAsyncPatternAsync")
        Using client As New WebClient()
            Dim content As String = Await client.DownloadStringTaskAsync(url)
            Console.WriteLine(content.Substring(0, 100))
            Console.WriteLine()
        End Using
    End Function

    Private Sub AsynchronousPattern()
        Console.WriteLine(NameOf(AsynchronousPattern))
        Dim request As WebRequest = WebRequest.Create(url)
        'Dim callback As AsyncCallback = AddressOf ReadResponse
        Dim callback As AsyncCallback =
            Sub(ar As IAsyncResult)
                Using response As WebResponse = request.EndGetResponse(ar)
                    Dim stream As Stream = response.GetResponseStream()
                    Dim reader = New StreamReader(stream)
                    Dim content As String = reader.ReadToEnd()
                    Console.WriteLine(content.Substring(0, 100))
                    Console.WriteLine()
                End Using
            End Sub
        Dim result As IAsyncResult = request.BeginGetResponse(callback, Nothing)
    End Sub

    Private Sub EventBasedAsyncPattern()
        Console.WriteLine(NameOf(EventBasedAsyncPattern))
        Using client = New WebClient()
            AddHandler client.DownloadStringCompleted,
                Sub(sender As Object, e As DownloadStringCompletedEventArgs)
                    Console.WriteLine(e.Result.Substring(0, 100))
                End Sub
            client.DownloadStringAsync(New Uri(url))
            Console.WriteLine()
        End Using
    End Sub

End Module
