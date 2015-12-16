/*
 * This interface is only so that any enemy or player that will be involved in fighting has a GUID so that it does not attack itself. 
 */

public interface ICombatant {
	string GetCombatantID();
}
