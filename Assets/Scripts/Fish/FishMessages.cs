using UnityEngine;

namespace Fish
{
    using System;
using System.Collections.Generic;

public static class FishMessages
{
    public static readonly string[] HungerComplaints =
    {
        "I am so hungry I might die soon...",
        "I cannot remember the last time I was fed...",
        "I am so hungry I can see my spine...",
        "I need food so bad… I am not even picky at this point...",
        "Please feed me...",
        "I am actually starving..."
    };

    public static readonly string[] SocialComplaints =
    {
        "Does no one love me?",
        "Am I really about to die alone...?",
        "All these fish and I still feel lonely...",
        "I miss my mom… Can you pretend to be her?",
        "I need to be touched...",
        "I have not felt the touch of a woman in years...",
        "If you do not pet me, I might just die..."
    };

    public static readonly Dictionary<int, List<string>> FeedingResponses = new()
    {
        { 10, new List<string> { "I was about to starve to death… You saved me!" } },
        { 20, new List<string> { "Thanks, I was so hungry I could eat another fish!" } },
        { 30, new List<string> { "Thanks, no one had fed me for so long..." } },
        { 40, new List<string> { "I was really hungry, thank you!" } },
        { 50, new List<string> { "Thanks for the meal!" } },
        { 60, new List<string> { "Delicious as always, thanks a lot!" } },
        { 70, new List<string> { "You are keeping me well fed, thanks!" } },
        { 80, new List<string> { "Thanks for the food, I love it here!" } },
        { 90, new List<string> { "You spoil me! Thank you for the food!" } },
        { 100, new List<string>
            {
                "Wow, I am literally full, thanks!",
                "That’s enough eating for today... Thanks!",
                "One more flake and I will explode! Thanks!"
            }
        }
    };

    public static readonly Dictionary<int, List<string>> PettingResponses = new()
    {
        { 10, new List<string> { "I thought I was about to die alone… You saved me!" } },
        { 20, new List<string> { "I was starting to spiral from loneliness, thanks..." } },
        { 30, new List<string> { "Wait! They don't love you like I love you..." } },
        { 40, new List<string> { "Can you pet me more?" } },
        { 50, new List<string> { "I wish you were a fish so we could swim together..." } },
        { 60, new List<string> { "You have really soft hands..." } },
        { 70, new List<string> { "If I am just pixels on the screen, why do I feel so loved?" } },
        { 80, new List<string> { "Is this what it feels like to be popular?" } },
        { 90, new List<string> { "Tell me I am the best fish without telling me I am the best fish..." } },
        { 100, new List<string>
            {
                "I am the alpha fish in this aquarium!",
                "I love fame, I do not see what is there to complain about...",
                "Will you love me after I die too?",
                "Thanks, I have never felt so loved...",
                "I will remember you forever..."
            }
        }
    };

    public static string GetRandomHungerComplaint()
    {
        return HungerComplaints[UnityEngine.Random.Range(0, HungerComplaints.Length)];
    }
    
    public static string GetRandomSocialComplaint()
    {
        return SocialComplaints[UnityEngine.Random.Range(0, SocialComplaints.Length)];
    }
    
    private static int NearestMultipleOf10(int num)
    {
        return Math.Max((int)Math.Round(num / 10.0) * 10, 10);
    }

    public static string GetFeedingResponse(int level)
    {
        level = NearestMultipleOf10(level);
        if (FeedingResponses.TryGetValue(level, out var responses))
        {
            return responses[UnityEngine.Random.Range(0, responses.Count)];
        }

        return "Thanks for feeding me!";
    }

    public static string GetPettingResponse(int level)
    {
        level = NearestMultipleOf10(level);
        if (PettingResponses.TryGetValue(level, out var responses))
        {
            return responses[UnityEngine.Random.Range(0, responses.Count)];
        }

        return "Thanks for petting me, I love you!";
    }
}

}