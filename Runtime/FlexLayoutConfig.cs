using Gilzoide.FlexUi.Yoga;
using UnityEngine;

namespace Gilzoide.FlexUi
{
    [CreateAssetMenu]
    public class FlexLayoutConfig : ScriptableObject
    {
        [Tooltip("If true, this configuration will be used by FlexLayout nodes that do not have a configuration set explicitly.")]
        [SerializeField] private bool _isDefaultConfig = false;

        [Tooltip("Yoga will by deafult round final layout positions and dimensions to the nearst point.\n"
            + "'Point Scale Factor' controls the density of the grid used for layout rounding (e.g. to round to the closest display pixel).\n"
            + "Set this to 0 to avoid rounding the layout results.")]
        [SerializeField, Min(0)] private float _pointScaleFactor = 1f;

        [Tooltip("Configures how Yoga balances W3C conformance vs compatibility with layouts created against earlier versions of Yoga.")]
        [SerializeField] private Errata _errata = Errata.None;

        [Header("Experimental Features")]
        [SerializeField] private bool _webFlexBasis = false;
        [SerializeField] private bool _absolutePercentageAgainstPaddingEdge = false;

        public YGConfig Config
        {
            get
            {
                if (_config.IsNull)
                {
                    if (_isDefaultConfig)
                    {
                        _config = YGConfig.GetDefaultConfig();
                    }
                    else
                    {
                        _config.Instantiate();
                    }
                    RefreshConfig();
                }
                return _config;
            }
        }

        private YGConfig _config;

        protected void OnDisable()
        {
            if (!_config.Equals(YGConfig.GetDefaultConfig()))
            {
                _config.Dispose();
            }
        }

        protected void RefreshConfig()
        {
            YGConfig config = Config;
            config.SetPointScaleFactor(_pointScaleFactor);
            config.SetErrata(_errata);
            config.SetExperimentalFeatureEnabled(ExperimentalFeature.WebFlexBasis, _webFlexBasis);
            config.SetExperimentalFeatureEnabled(ExperimentalFeature.AbsolutePercentageAgainstPaddingEdge, _absolutePercentageAgainstPaddingEdge);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            RefreshConfig();
        }
#endif
    }
}
