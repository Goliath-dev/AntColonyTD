using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpace<T>
{
    T AntToSpace(Ant ant);
    T NodeToSpace(Node node);
}
