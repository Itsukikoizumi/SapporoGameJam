using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    /// <summary>
    /// 操作を止める原因のタイプリスト
    /// </summary>
    public enum BlockType
    {
        SceneManager,
        GameOver,
    }

    private EventSystem eventSystem;
    private List<BlockType> inputBlocks = new List<BlockType>();

    protected override void doAwake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    /// <summary>
    /// 操作を止める
    /// </summary>
    /// <param name="type">
    public void BlockInput(BlockType type)
    {
        inputBlocks.Add(type);
        eventSystem.enabled = false;
    }

    public void UnblockInput(BlockType type)
    {
        inputBlocks.Remove(type);
        if (inputBlocks.Count == 0)
        {
            eventSystem.enabled = true;
        }
    }
}