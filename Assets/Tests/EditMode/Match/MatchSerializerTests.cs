using Chess;
using NUnit.Framework;
using System.IO;
using UnityEngine;

public class MatchSerializerTest
{

    [Test]
    public void hikaru_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.dataPath + "/Tests/MatchFiles" + "/Hikaru Nakamura_vs_Nikita Matinian_2023.01.03.pgn"));
    }

    [Test]
    public void magnus_carlsen_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.dataPath + "/Tests/MatchFiles" + "/Magnus Carlsen_vs_Brandon Jacobson_2023.01.03.pgn"));
    }

    [Test]
    public void bobby_fischer_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.dataPath + "/Tests/MatchFiles" + "/Bobby Fischer_vs_Boris V Spassky_1992. . .pgn"));
    }

    [Test]
    public void capablanca_match_does_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.dataPath + "/Tests/MatchFiles" + "/Jose Raul Capablanca_vs_Vladas Mikenas_1939. . .pgn"));
    }

    [Test]
    public void en_passant_do_not_throw()
    {
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.dataPath + "/Tests/MatchFiles" + "/gunnar-gundersen_vs_a-h-faul_1928.pgn"));
        Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(Application.dataPath + "/Tests/MatchFiles" + "/magnus-carlsen_vs_loek-van-wely_29-apr-2006.pgn"));
    }

    [Test]
    public void custom_games_do_not_throw()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Tests/MatchFiles/CustomGames");
        foreach (string file in files)
        {
            if (file.EndsWith(".pgn"))
            {
                Assert.DoesNotThrow(() => MatchSerializer.DeserializeMatch(file));
            }
        }
    }
}
