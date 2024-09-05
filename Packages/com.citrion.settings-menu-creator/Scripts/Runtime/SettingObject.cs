using CitrioN.Common;
using UnityEngine;

namespace CitrioN.SettingsMenuCreator
{
  [AddTooltips]
  [SkipObfuscationRename]
  [HeaderInfo("Allows the specification of a string to identify this object within the settings menu system and has a lot of use cases.\n\n" +
              "Used to create a connection between this object and a setting that uses the same identifier. " +
              "The settings menu automatically assigns this script and assign the correct identifier if the input element (dropdown, slider etc.) " +
              "is generated by the system. If you don't want an input element to be generated you can assign this manually.\n\n" +
              "Example:\n" +
              "If a setting has an identifier named 'screen-resolution' you can use the same identifier on something like a dropdown to allow " +
              "the settings system to find and connect the dropdown to that setting.\n\n" +
              "Also used for things like parenting a setting input element by specifying a settings parent. " +
              "Each setting has a 'Parent Identifier' field in the advanced tab. By default this is named 'settings-parent'. " +
              "This is used to find the corresponding SettingObject and parent the input element to that object. " +
              "In the provided menu layout prefabs there is already an object with the matching identifier of 'settings-parent' " +
              "but you can of course create your own as you see fit for your menu(s).")]
  [AddComponentMenu("CitrioN/Settings Menu Creator/Setting Object (UGUI)")]
  public class SettingObject : MonoBehaviour
  {
    [SerializeField]
    [SkipObfuscationRename]
    [Tooltip("The identifier used for this object.\n\n" +
             "If assigned a setting with the same identifier " +
             "can be used to create a connection to this object. \n\n" +
             "Can be used to identify a setting parent object.")]
    protected string identifier = null;

    [SerializeField]
    [Tooltip("The transform to attach elements to for this setting object. " +
             "If left empty elements will be attached to this transform.")]
    protected Transform contentParent;

    [SkipObfuscationRename]
    public string Identifier { get => identifier; set => identifier = value; }

    public Transform GetContentParent()
    {
      return contentParent != null ? contentParent : transform;
    }
  }
}