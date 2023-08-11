using MouseAndCreate.Graphics;
using MouseAndCreate.Types;
using System;

namespace MouseAndCreate.Objects
{
    public class ImageBackgroundObject : BackgroundObject, IAssignable<ImageBackgroundObject>
    {
        public Image32 Image { get; set; }

        public ImageBackgroundObject(Image32 image = null, Guid? id = null, string name = null) : base(id, name)
        {
            Image = image;
        }

        public void Assign(ImageBackgroundObject other)
        {
            base.Assign(other as BackgroundObject);
            if (other is null)
                return;
            Image = other.Image;
        }

        public override IGameObject Clone()
        {
            ImageBackgroundObject result = new ImageBackgroundObject();
            result.Assign(this);
            return result;
        }

        protected override void DisposeManaged()
        {
            Image.Dispose();
        }
    }
}
