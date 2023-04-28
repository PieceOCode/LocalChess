using Chess;
using NUnit.Framework;
using UnityEngine;

public class MatchSerializerTest
{
    [Test]
    public void example_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.persistentDataPath + "/example.pgn"));
    }

    [Test]
    public void hikaru_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.persistentDataPath + "/Hikaru Nakamura_vs_Nikita Matinian_2023.01.03.pgn"));
    }

    [Test]
    public void magnus_carlsen_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.persistentDataPath + "/Magnus Carlsen_vs_Brandon Jacobson_2023.01.03.pgn"));
    }

    [Test]
    public void bobby_fischer_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.persistentDataPath + "/Bobby Fischer_vs_Boris V Spassky_1992. . .pgn"));
    }

    [Test]
    public void capablanca_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.persistentDataPath + "/Jose Raul Capablanca_vs_Vladas Mikenas_1939. . .pgn"));
    }
}
