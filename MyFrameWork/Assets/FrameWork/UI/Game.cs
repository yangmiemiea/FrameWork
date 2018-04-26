using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : BaseManager<Game>
{
    public Transform UILayer { get { return GameObject.Find("UILayer").transform; } }
}
