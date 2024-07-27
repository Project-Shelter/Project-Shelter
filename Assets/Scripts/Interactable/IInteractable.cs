public interface IInteractable
{
    // EnterState
    void StartInteract(Actor actor);
    // UpdateState
    void Interacting();
    // ExitState
    void StopInteract();
    // UpdateState
    bool CanKeepInteracting();
}
