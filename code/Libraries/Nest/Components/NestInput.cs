using System;
using System.Threading.Tasks;
using Nest.Util;
using Sandbox;

namespace Nest.Components 
{ 
    public delegate void CastEvent(NestInput input);

    [Group("Nest/Components/Nest Input")]
    public class NestInput : Component
    {
        public event CastEvent EventFired;

        public bool HasFired
        {
            get
            {
                return _fired;
            }
        }

        public EventType CurrentEventType
        {
            get
            {
                return _eventType;
            }
            set
            {
                _eventType = value;
            }
        }

        /*
        public UnityEventBase CurrentEvent
        {
            get
            {
                EventType eventType = _eventType;

                switch (eventType)
                {
                    case EventType.Trigger:
                        return _event;

                    case EventType.Bool:
                        break;

                    case EventType.Trigger | EventType.Bool:
                        goto IL_72;

                    case EventType.Float:
                        return _eventValue;

                    default:
                        if (eventType != EventType.Toggle)
                        {
                            if (eventType != EventType.String)
                            {
                                goto IL_72;
                            }

                            return _eventString;
                        }

                        break;
                }

                if (Value.CurrentValue < 0.01f)
                {
                    return _event;
                }

                if (Value.CurrentValue > 0.99f)
                {
                    return _event2;
                }

            IL_72:
                return _event;
            }
        }
        */

        public bool EventPosition
        {
            get
            {
                return MathF.Abs(Value.CurrentValue) < 0.01f;
            }

            set
            {
                Value.CurrentValue = (value ? 1 : 0);
            }
        }

        protected override void OnStart()
        {
            Value = new FloatInterpolator(_eventOffValue, _eventOnValue, _interpolation);
        }

        public async void Invoke()
        {
            if ((FireOnce && _fired) || !Enabled)
            {
                return;
            }

            _fired = true;

            if (Delay > 0f)
            {
                await InvokeDelay();
                return;
            }

            InvokeEvent();
        }

        private async Task InvokeDelay()
        {
            await GameTask.DelaySeconds((int)Delay);
            InvokeEvent();
        }

        public void InvokeEvent()
        {
            EventType eventType = _eventType;

            switch (eventType)
            {
                case EventType.Trigger:
                    // _event.Invoke();
                    break;

                case EventType.Bool:
                    if (MathF.Abs(Value.CurrentValue) < 0.01f)
                    {
                        // _event2.Invoke();
                    }
                    else if (MathF.Abs(Value.CurrentValue) - 1f < 0.01f)
                    {
                        // _event.Invoke();
                    }

                    break;

                case EventType.Trigger | EventType.Bool:
                    break;

                case EventType.Float:
                    // _eventValue.Invoke(_parameterFloat);
                    break;

                default:
                    if (eventType != EventType.Toggle)
                    {
                        if (eventType == EventType.String)
                        {
                            // _eventString.Invoke(_parameterString);
                        }
                    }
                    else
                    {
                        Value.CurrentValue = 1f - Value.CurrentValue;

                        if (MathF.Abs(Value.CurrentValue) < 0.01f)
                        {
                            // _event2.Invoke();
                        }
                        else
                        {
                            // _event.Invoke();
                        }
                    }

                    break;
            }

            if (EventFired != null)
            {
                EventFired(this);
            }
        }

        /*
        public int[] FindGameObjectsReferences()
        {
            float currentValue = Value.CurrentValue;
            Value.CurrentValue = 0f;
            int num = CurrentEvent.GetPersistentEventCount();

            if (_eventType == EventType.Bool || _eventType == EventType.Toggle)
            {
                num += _event2.GetPersistentEventCount();
            }

            int[] array = new int[num];

            for (int i = 0; i < num; i++)
            {
                object @object = null;

                if (i > CurrentEvent.GetPersistentEventCount())
                {
                    @object = _event2.GetPersistentTarget(i - CurrentEvent.GetPersistentEventCount());
                }
                else if (CurrentEvent.GetPersistentEventCount() > i)
                {
                    @object = CurrentEvent.GetPersistentTarget(i);
                }
                else if (_event2.GetPersistentEventCount() > i)
                {
                    @object = _event2.GetPersistentTarget(i);
                }

                if (@object is GameObject)
                {
                    array[i] = @object.GetHashCode();
                }
                else if (@object is Component)
                {
                    array[i] = ((Component)@object).GameObject.GetHashCode();
                }
            }

            Value.CurrentValue = currentValue;
            return array;
        }
        */

        public void SetBool(bool value)
        {
            Value.TargetValue = (value ? _eventOnValue : _eventOffValue);
            Value.CurrentValue = Value.TargetValue;
        }

        [Property]
        private EventType _eventType { get; set; } = EventType.Trigger;

        [Property]
        private float _eventOffValue { get; set; }

        [Property]
        private float _eventOnValue { get; set; } = 1f;

        /*
        [Property]
        public UnityEvent _event { get;set; }

        [Property]
        private UnityEvent _event2 { get;set; }

        [Property]
        public ValueEvent _eventValue { get;set; }
        */

        [Property]
        public float _parameterFloat { get; set; }

        /*
        [Property]
        public StringEvent _eventString { get;set; }
        */

        [Property]
        public string _parameterString { get; set; }

        [Property]
        public bool FireOnce { get; set; }

        [Property]
        private FloatInterpolator.Config _interpolation { get; set; }

        private bool _fired;

        [Property]
        public float Delay { get; set; }

        protected FloatInterpolator Value;

        [Flags]
        public enum EventType
        {
            Trigger = 1,
            Bool = 2,
            Float = 4,
            Toggle = 16,
            String = 32
        }
    }
}