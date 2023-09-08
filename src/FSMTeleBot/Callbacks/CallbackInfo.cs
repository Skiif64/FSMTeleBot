using System.Collections;

namespace FSMTeleBot.Callbacks;
public readonly record struct CallbackInfo : IEnumerable<string>
{
    public string Header { get; } = string.Empty;
    public string[] Data { get; } = Array.Empty<string>();

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
