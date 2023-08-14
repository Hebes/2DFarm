
namespace ACFrameworkCore
{
    public class AboutPanel : UIBase
    {
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.PopUp, EUIMode.ReverseChange, EUILucenyType.Translucence);

            ButtonOnClickAddListener("Back",
              p =>
              {
                  CloseUIForm();
              });
        }
    }
}
