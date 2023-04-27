using System.Collections;

namespace FSMTeleBot.Callbacks;
public class CallbackInfo : IEnumerable<string>
{
    public string Header { get; }
    public string[] Data { get; }

    internal CallbackInfo(string header, string[] data)
    {
        Header = header;
        Data = data;
    }

    internal CallbackInfo(string header, IEnumerable<string> data)
        : this(header, data.ToArray())
    {

    }

    internal CallbackInfo(string header)
        : this(header, Array.Empty<string>())
    {

    }
    public IEnumerator<string> GetEnumerator()
    {
        foreach (var data in Data)
        {
            yield return data;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    => Data.GetEnumerator();

}
