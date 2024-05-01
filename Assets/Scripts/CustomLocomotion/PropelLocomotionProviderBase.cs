using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.Interaction.Toolkit;

namespace CustomLocomotion
{
    public abstract class PropelLocomotionProviderBase : LocomotionProvider
    {
        [SerializeField]
        [Tooltip("The speed, in units per second, to move forward.")]
        protected float moveSpeed = 1f;
        
        [SerializeField]
        [Tooltip("The source Transform to define the forward direction.")]
        private Transform mForwardSource;
 
        public Transform forwardSource
        {
            get => mForwardSource;
            set => mForwardSource = value;
        }

        private Rigidbody xrRigidBody;
        private bool attemptedGetRigidBody;
        private bool isMovingXROrigin;
        private Vector3 verticalVelocity;
        
        protected void Update()
        {
            isMovingXROrigin = false;

            var xrOrigin = system.xrOrigin?.Origin;
            if (xrOrigin == null)
                return;

            Vector3 input = ReadInput();
            var translationInWorldSpace = ComputeDesiredMove(input);
            MoveRig(translationInWorldSpace);
            
            switch (locomotionPhase)
            {
                case LocomotionPhase.Idle:
                case LocomotionPhase.Started:
                    if (isMovingXROrigin)
                        locomotionPhase = LocomotionPhase.Moving;
                    break;
                case LocomotionPhase.Moving:
                    if (!isMovingXROrigin)
                        locomotionPhase = LocomotionPhase.Done;
                    break;
                case LocomotionPhase.Done:
                    locomotionPhase = isMovingXROrigin ? LocomotionPhase.Moving : LocomotionPhase.Idle;
                    break;
                default:
                    Assert.IsTrue(false, $"Unhandled {nameof(LocomotionPhase)}={locomotionPhase}");
                    break;
            }
        }

        /// <summary>
        /// Reads the current value of the move input.
        /// </summary>
        /// <returns>Returns the input vector, such as from a thumbstick.</returns>
        protected abstract Vector3 ReadInput();

        /// <summary>
        /// Determines how much to slide the rig due to <paramref name="input"/> vector.
        /// </summary>
        /// <param name="input">Input vector, such as from a thumbstick.</param>
        /// <returns>Returns the translation amount in world space to move the rig.</returns>
        protected virtual Vector3 ComputeDesiredMove(Vector3 input)
        {
            if (input == Vector3.zero)
                return Vector3.zero;

            var xrOrigin = system.xrOrigin;
            if (xrOrigin == null)
                return Vector3.zero;
            
            var inputMove = Vector3.ClampMagnitude(new Vector3(input.x, input.y, input.z), 1f);

            // Determine frame of reference for what the input direction is relative to
            var forwardSourceTransform = mForwardSource == null ? xrOrigin.Camera.transform : mForwardSource;
            var inputForwardInWorldSpace = forwardSourceTransform.forward;

            var originTransform = xrOrigin.Origin.transform;
            var speedFactor = moveSpeed * Time.deltaTime * originTransform.localScale.x;
            var originUp = originTransform.up;

            if (Mathf.Approximately(Mathf.Abs(Vector3.Dot(inputForwardInWorldSpace, originUp)), 1f))
            {
                inputForwardInWorldSpace = -forwardSourceTransform.up;
            }

            var inputForwardProjectedInWorldSpace = Vector3.ProjectOnPlane(inputForwardInWorldSpace, originUp);
            var forwardRotation = Quaternion.FromToRotation(originTransform.forward, inputForwardProjectedInWorldSpace);

            var translationInRigSpace = forwardRotation * inputMove * speedFactor;
            var translationInWorldSpace = originTransform.TransformDirection(translationInRigSpace);

            return translationInWorldSpace;
        }

        /// <summary>
        /// Creates a locomotion event to move the rig by <paramref name="translationInWorldSpace"/>,
        /// and optionally applies gravity.
        /// </summary>
        /// <param name="translationInWorldSpace">The translation amount in world space to move the rig (pre-gravity).</param>
        protected virtual void MoveRig(Vector3 translationInWorldSpace)
        {
            var xrOrigin = system.xrOrigin?.Origin;
            if (xrOrigin == null)
                return;

            FindRigidBody();
            
            if (xrRigidBody == null || xrRigidBody.isKinematic) return;
            

            if (!CanBeginLocomotion() || !BeginLocomotion()) return;
            isMovingXROrigin = true;
            xrRigidBody.AddForce(translationInWorldSpace);
            EndLocomotion();
        }

        private void FindRigidBody()
        {
            var xrOrigin = system.xrOrigin?.Origin;
            if (xrOrigin == null || xrRigidBody != null || attemptedGetRigidBody) return;
            if (!xrOrigin.TryGetComponent(out xrRigidBody) && xrOrigin != system.xrOrigin.gameObject)
                system.xrOrigin.TryGetComponent(out xrRigidBody);
            attemptedGetRigidBody = true;
        }
    }
}