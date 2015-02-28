using Assets.Classes.Core;

namespace Assets.Classes.Implementation
{
    public class UITop : SingletonEntity<UITop>
    {
        public enum TopContext
        {
            Title,
            Description
        }

        public TopContext CurrentContext { get; private set; }


        public string OpenDescriptionAnimationName = "ui_title_d_open";
        public string CloseDescriptionAnimationName = "ui_title_d_close";

        public void SetContext(string animationName, TopContext context)
        {
            animator.Play(animationName);
            CurrentContext = context;
        }

        public void OnTopClick()
        {
            if(CurrentContext == TopContext.Title) SetContext(OpenDescriptionAnimationName, TopContext.Description);
            else if(CurrentContext == TopContext.Description) SetContext(CloseDescriptionAnimationName, TopContext.Title);
        }

        protected override void Awake()
        {
            CurrentContext = TopContext.Title;
            base.Awake();
        }
    }
}
