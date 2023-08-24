using System;
using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace UnityAtoms.Tags
{
    /// <summary>
    /// A MonoBehaviour that adds tags the Unity Atoms way to a GameObject.
    /// </summary>
    [EditorIcon("atom-icon-delicate")]
    [AddComponentMenu("Unity Atoms/Tags")]
    public sealed class AtomTags : MonoBehaviour
    {
        /// <summary>
        /// Get the tags associated with this GameObject as `StringConstants` in a `ReadOnlyList&lt;T&gt;`.
        /// </summary>
        /// <value>The tags associated with this GameObject as `StringConstants` in a `ReadOnlyList&lt;T&gt;`.</value>
        public ReadOnlyList<StringConstant> Tags { get => new(_tags); private set => _readOnlyTags = value; }

        private ReadOnlyList<StringConstant> _readOnlyTags;

        [SerializeField]
        private List<StringConstant> _tags = new();

        static private readonly Dictionary<string, List<GameObject>> TaggedGameObjects = new();

        static private readonly Dictionary<GameObject, AtomTags> TagInstances = new();
        static private Action _onInitialization;

        static private List<GameObject> _returnList = new();

#region Lifecycles

        private void OnEnable()
        {
            if (IsInitialized(gameObject)) return;

            TagInstances.Add(gameObject, this);

            for (int i = 0; i < Tags.Count; i++)
            {
                StringConstant stringConstant = Tags[i];
                if (stringConstant == null) continue;
                string tag = stringConstant.Value;
                if (!TaggedGameObjects.ContainsKey(tag)) TaggedGameObjects.Add(tag, new());
                TaggedGameObjects[tag].Add(gameObject);
            }

            _onInitialization?.Invoke();
            _onInitialization = null;
        }

        private void OnDestroy()
        {
            if (TagInstances.ContainsKey(gameObject)) TagInstances.Remove(gameObject);
            foreach (StringConstant stringConstant in Tags)
            {
                if (stringConstant == null) continue;
                string tag = stringConstant.Value;
                if (TaggedGameObjects.TryGetValue(tag, out List<GameObject> gameObjects)) gameObjects.Remove(gameObject);
            }
        }

#endregion

        static public void OnInitialization(Action handler)
        {
            AtomTags atomTags = FindObjectOfType<AtomTags>();
            if (atomTags != null && !IsInitialized(atomTags.gameObject))
                _onInitialization += handler;
            else
                handler();
        }

        /// <summary>
        /// Check if the tag provided is associated with this `GameObject`.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>`true` if the tag exists, otherwise `false`.</returns>
        public bool HasTag(StringConstant tag)
        {
            if (tag == null) return false;
            return _tags.Contains(tag);
        }

        /// <summary>
        /// Add a tag to this `GameObject`.
        /// </summary>
        /// <param name="tag">The tag to add as a `StringContant`.</param>
        public void AddTag(StringConstant tag)
        {
            if (tag == null || tag.Value == null) return;
            if (_tags.Contains(tag)) return;
            _tags.Add(tag);

            Tags = new(_tags);

            // Update static accessors:
            if (!TaggedGameObjects.ContainsKey(tag.Value)) TaggedGameObjects.Add(tag.Value, new());
            TaggedGameObjects[tag.Value].Add(gameObject);
        }

        /// <summary>
        /// Remove a tag from this `GameObject`.
        /// </summary>
        /// <param name="tag">The tag to remove as a `string`</param>
        public void RemoveTag(StringConstant tag)
        {
            if (tag == null) return;
            if (!_tags.Contains(tag)) return;
            _tags.Remove(tag);

            Tags = new(_tags);

            // Update static accessors:
            if (!TaggedGameObjects.ContainsKey(tag.Value)) return; // this should never happen
            TaggedGameObjects[tag.Value].Remove(gameObject);
        }

        /// <summary>
        /// Find first `GameObject` that has the tag provided.
        /// </summary>
        /// <param name="tag">The tag that the `GameObject` that you search for will have.</param>
        /// <param name="includeInactive">Is it suppose to add inactive GameObjects?</param>
        /// <returns>The first `GameObject` with the provided tag found. If no `GameObject`is found, it returns `null`.</returns>
        static public GameObject FindByTag(string tag, bool includeInactive = false)
        {
            if (!TaggedGameObjects.ContainsKey(tag)) return null;
            if (TaggedGameObjects[tag].Count < 1) return null;

            _returnList.Clear();

            foreach (GameObject gameObject in TaggedGameObjects[tag])
                if (gameObject.activeInHierarchy)
                    _returnList.Add(gameObject);

            return includeInactive
                ? TaggedGameObjects[tag][0]
                : _returnList.Count > 0
                    ? _returnList[0]
                    : null;
        }

        /// <summary>
        /// Find all `GameObject`s that have the tag provided.
        /// </summary>
        /// <param name="tag">The tag that the `GameObject`s that you search for will have.</param>
        /// <param name="includeInactive">Is it suppose to add inactive GameObjects?</param>
        /// <returns>An array of `GameObject`s with the provided tag. If not found it returns `null`.</returns>
        static public List<GameObject> FindAllByTag(string tag, bool includeInactive = false)
        {
            if (!TaggedGameObjects.ContainsKey(tag)) return null;

            if (includeInactive) { _returnList = TaggedGameObjects[tag]; } else
            {
                _returnList.Clear();
                foreach (GameObject gameObject in TaggedGameObjects[tag])
                    if (gameObject.activeInHierarchy)
                        _returnList.Add(gameObject);
            }

            return _returnList;
        }

        /// <summary>
        /// Find all `GameObject`s that have the tag provided. Mutates the output `List&lt;GameObject&gt;` and adds the `GameObject`s found to it.
        /// </summary>
        /// <param name="tag">The tag that the `GameObject`s that you search for will have.</param>
        /// <param name="output">A `List&lt;GameObject&gt;` that this method will clear and add the `GameObject`s found to.</param>
        static public void FindAllByTagNoAlloc(string tag, List<GameObject> output)
        {
            output.Clear();
            if (!TaggedGameObjects.ContainsKey(tag)) return;
            for (int i = 0; i < TaggedGameObjects[tag].Count; ++i) output.Add(TaggedGameObjects[tag][i]);
        }

        /// <summary>
        /// A faster alternative to `gameObject.GetComponen&lt;UATags&gt;()`.
        /// </summary>
        /// <returns>
        /// Returns the `UATags` component. Returns `null` if the `GameObject` does not have a `UATags` component or if the `GameObject` is disabled.
        /// </returns>
        static public AtomTags GetTagsForGameObject(GameObject go)
        {
            if (!TagInstances.ContainsKey(go)) return null;
            return TagInstances[go];
        }

        /// <summary>
        /// Retrieves all tags for a given `GameObject`. A faster alternative to `gameObject.GetComponen&lt;UATags&gt;().Tags`.
        /// </summary>
        /// <param name="go">The `GameObject` to check for tags.</param>
        /// <returns>
        /// A `ReadOnlyList&lt;T&gt;` of tags stored as `StringContant`s. Returns `null` if the `GameObject` does not have any tags or if the `GameObject` is disabled.
        /// </returns>
        static public ReadOnlyList<StringConstant> GetTags(GameObject go)
        {
            if (!TagInstances.ContainsKey(go)) return null;
            AtomTags tags = TagInstances[go];
            return tags.Tags;
        }

        static private bool IsInitialized(GameObject go) { return TagInstances.ContainsKey(go); }
    }
}
