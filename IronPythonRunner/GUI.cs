using CSR;
using Newtonsoft.Json.Linq;

namespace GUI
{
    public class GUIBuilder
    {
        private MCCSAPI api { get; set; }
        private JObject gui { get; set; }
        private JArray content { get; set; }
        public GUIBuilder(MCCSAPI mcapi, string title)
        {
            this.api = mcapi;
            this.gui = new JObject();
            BuildNewGUI(title);
        }
        private void BuildNewGUI(string title)
        {
            this.gui = new JObject();
            gui.Add(new JProperty("type", "custom_form"));
            gui.Add(new JProperty(nameof(title), title));
            content = new JArray();
        }
        /// <summary>
        /// 添加一串文字
        /// </summary>
        /// <param name="text">文字</param>
        public void AddLabel(string text)
        {
            content.Add(new JObject
            {
                new JProperty("type", "label"),
                new JProperty(nameof(text), text)
            });
        }
        /// <summary>
        /// 添加一个输入框
        /// </summary>
        /// <param name="text">描述</param>
        /// <param name="placeholder">输入框背景文字</param>
        public void AddInput(string text, string placeholder = "")
        {
            content.Add(new JObject
            {
                new JProperty("type", "input"),
                new JProperty(nameof(placeholder), placeholder),
                new JProperty("default",""),
                new JProperty("text",text)
            });
        }
        /// <summary>
        /// 添加一个开关
        /// </summary>
        /// <param name="text">描述</param>
        /// <param name="_default">默认开关状态</param>
        public void AddToggle(string text, bool _default = false)
        {
            content.Add(new JObject
            {
                new JProperty("type", "toggle"),
                new JProperty("default",_default),
                new JProperty(nameof(text), text)
            });
        }
        /// <summary>
        /// 添加一个游标滑块
        /// </summary>
        /// <param name="text">描述</param>
        /// <param name="min">滑块最小值</param>
        /// <param name="max">滑块最大值</param>
        /// <param name="step">滑动一次移动的格数</param>
        /// <param name="_default">默认数值</param>
        public void AddSlider(string text, int min = 0, int max = 100, int step = 1, int _default = 0)
        {
            content.Add(new JObject
            {
                new JProperty("type", "slider"),
                new JProperty("default",_default),
                new JProperty(nameof(text), text),
                new JProperty(nameof(min),min),
                new JProperty(nameof(max),max),
                new JProperty(nameof(step),step)
            });
        }
        /// <summary>
        /// 添加一个矩阵滑块
        /// </summary>
        /// <param name="text">描述</param>
        /// <param name="_default">默认值</param>
        /// <param name="options">选项</param>
        public void AddStepSlider(string text, int _default, string options)
        {
            var t = new JArray();
            options = options.Replace("[", null);
            options = options.Replace("]", null);
            options = options.Replace("\'", null);
            string[] strArray = options.Split(',');
            foreach (var i in strArray) t.Add(i);
            content.Add(new JObject
            {
                new JProperty("type", "step_slider"),
                new JProperty("default",_default),
                new JProperty(nameof(text), text),
                new JProperty("steps",t)
            });
        }
        /// <summary>
        /// 添加一个下拉框
        /// </summary>
        /// <param name="text">描述</param>
        /// <param name="_default">默认值</param>
        /// <param name="options">选项</param>
        public void AddDropdown(string text, int _default, string options)
        {
            var t = new JArray();
            options = options.Replace("[", null);
            options = options.Replace("]", null);
            options = options.Replace("\'", null);
            string[] strArray = options.Split(','); //字符串转数组
            foreach (var i in strArray) t.Add(i);
            content.Add(new JObject
            {
                new JProperty("type", "dropdown"),
                new JProperty("default",_default),
                new JProperty(nameof(text), text),
                new JProperty(nameof(options),t)
            });
        }
        /// <summary>
        /// 发送给玩家
        /// </summary>
        /// <param name="uuid">玩家uuid</param>
        /// <returns></returns>
        public uint SendToPlayer(string uuid)
        {
            gui.Add(new JProperty(nameof(content), content));
            return api.sendCustomForm(uuid, gui.ToString());
        }
    }
}
