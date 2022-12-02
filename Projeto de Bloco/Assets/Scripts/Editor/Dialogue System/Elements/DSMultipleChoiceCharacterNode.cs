using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace DS.Elements
{
    using Enumerations;
    using Windows;
    using Utilities;
    public class DSMultipleChoiceCharacterNode : DSMultipleChoiceNode
    {
        public string CharacterName { get; set; }
        public Sprite CharacterSprite { get; set; }
        public override void Initialize(DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(dsGraphView, position);
            
            CharacterName = "Character Name";
            
            DialogueType = DialogueType.MultipleChoiceCharacter;
            
            defalutBackgroundColor = Color.yellow;
            ResetStyle();
        }

        public override void Draw()
        {
            base.Draw();

            TextField _characterNameTextField = DSElementUtility.CreateTextField(CharacterName, null, callback =>
            {
                CharacterName = callback.newValue;
            });
            _characterNameTextField.AddClasses("ds-node__text-field", 
                "ds-node__character-name-text-field",
                "ds-node__text-field__hidden");
            extensionContainer.Insert(0, _characterNameTextField);
            
            ObjectField _characterSpriteField = new ObjectField()
            {
                objectType = typeof(Sprite),
                allowSceneObjects = false,
                value = CharacterSprite
            };
            _characterSpriteField.AddToClassList("ds-node__object-field");
            
            Image _spriteImage = new Image()
            {
                sprite = CharacterSprite,
                scaleMode = ScaleMode.ScaleToFit
            };
            _spriteImage.AddToClassList("ds-node__image");

            _characterSpriteField.RegisterValueChangedCallback(value =>
            {
                CharacterSprite = value.newValue as Sprite;
                _spriteImage.sprite = value.newValue as Sprite;
            });
            extensionContainer.Insert(1, _characterSpriteField);
            extensionContainer.Insert(1, _spriteImage);
            
        }
    }
}