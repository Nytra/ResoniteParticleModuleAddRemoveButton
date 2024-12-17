using FrooxEngine;
using MonkeyLoader.Resonite.UI;
using FrooxEngine.PhotonDust;
using FrooxEngine.UIX;

namespace ParticleModuleAddRemoveButton
{
    internal class ParticleSystemModuleInspectorHandler : ResoniteInspectorMonkey<ParticleSystemModuleInspectorHandler, BuildInspectorBodyEvent>
    {
        public ParticleSystemModuleInspectorHandler() : base(typeof(ParticleSystemModule<>))
        { }

        public override int Priority => HarmonyLib.Priority.First;
        protected override void Handle(BuildInspectorBodyEvent eventData)
        {
            ParticleInspectorHelper.Handle(eventData);
        }
    }

    internal class ParticleSystemRendererModuleInspectorHandler : ResoniteInspectorMonkey<ParticleSystemRendererModuleInspectorHandler, BuildInspectorBodyEvent>
    {
        public ParticleSystemRendererModuleInspectorHandler() : base(typeof(ParticleSystemRendererModule<,>))
        { }

        public override int Priority => HarmonyLib.Priority.First;
        protected override void Handle(BuildInspectorBodyEvent eventData)
        {
            ParticleInspectorHelper.Handle(eventData);
        }
    }

    internal static class ParticleInspectorHelper
    {
        public static void Handle(BuildInspectorBodyEvent eventData)
        {
            var particleStyle = eventData.Worker.FindNearestParent<Slot>().GetComponentInParents<ParticleStyle>();
            string text = "No ParticleStyle Found";
            if (particleStyle != null)
            {
                if (particleStyle.Modules.Contains((IParticleSystemSubsystem)eventData.Worker))
                {
                    text = "Remove Module";
                }
                else
                {
                    text = "Add Module";
                }
            }
            var button = eventData.UI.LocalActionButton(text, (Button btn) =>
            {
                var particleStyle = eventData.Worker.FindNearestParent<Slot>().GetComponentInParents<ParticleStyle>();
                string text = "No ParticleStyle Found";
                if (particleStyle != null)
                {
                    if (particleStyle.Modules.Contains((IParticleSystemSubsystem)eventData.Worker))
                    {
                        particleStyle.Modules.Remove((IParticleSystemSubsystem)eventData.Worker);
                        text = "Add Module";
                    }
                    else
                    {
                        particleStyle.Modules.Add((IParticleSystemSubsystem)eventData.Worker);
                        text = "Remove Module";
                    }
                }
                btn.LabelText = text;
            });
            if (particleStyle != null)
            {
                particleStyle.Modules.Changed += (IChangeable changeable) =>
                {
                    if (particleStyle.Modules.Contains((IParticleSystemSubsystem)eventData.Worker))
                    {
                        button.LabelText = "Remove Module";
                    }
                    else
                    {
                        button.LabelText = "Add Module";
                    }
                };
            }
        }
    }
}