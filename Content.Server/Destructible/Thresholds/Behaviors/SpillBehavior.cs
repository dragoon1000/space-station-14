using Content.Server.Chemistry.EntitySystems;
using Content.Server.Fluids.Components;
using Content.Server.Fluids.EntitySystems;
using JetBrains.Annotations;
using Robust.Shared.GameObjects;
using Robust.Shared.Serialization.Manager.Attributes;

namespace Content.Server.Destructible.Thresholds.Behaviors
{
    [UsedImplicitly]
    [DataDefinition]
    public class SpillBehavior : IThresholdBehavior
    {
        [DataField("solution")]
        public string? Solution;

        /// <summary>
        /// If there is a SpillableComponent on IEntity owner use it to create a puddle/smear.
        /// Or whatever solution is specified in the behavior itself.
        /// If none are available do nothing.
        /// </summary>
        /// <param name="owner">Entity on which behavior is executed</param>
        /// <param name="system">system calling the behavior</param>
        public void Execute(EntityUid owner, DestructibleSystem system)
        {
            var solutionContainerSystem = EntitySystem.Get<SolutionContainerSystem>();
            var spillableSystem = EntitySystem.Get<SpillableSystem>();

            var coordinates = system.EntityManager.GetComponent<TransformComponent>(owner).Coordinates;

            if (system.EntityManager.TryGetComponent(owner, out SpillableComponent? spillableComponent) &&
                solutionContainerSystem.TryGetSolution(owner, spillableComponent.SolutionName,
                    out var compSolution))
            {
                spillableSystem.SpillAt(compSolution, coordinates, "PuddleSmear", false);
            }
            else if (Solution != null &&
                     solutionContainerSystem.TryGetSolution(owner, Solution, out var behaviorSolution))
            {
                spillableSystem.SpillAt(behaviorSolution, coordinates, "PuddleSmear", false);
            }
        }
    }
}
