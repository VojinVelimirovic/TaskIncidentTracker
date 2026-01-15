import { useState, useEffect } from "react";
import "../../styles/TaskModal.css";

export default function TaskModal({ task, allUsers, onClose, onSave }) {
  const [status, setStatus] = useState(task.status);
  const [assignedUsers, setAssignedUsers] = useState(task.assignedTo.map(u => u.id));
  const [showUserSelect, setShowUserSelect] = useState(false);

  const availableUsers = allUsers.filter(u => !assignedUsers.includes(u.id));

  const handleAddUser = (userId) => {
    setAssignedUsers([...assignedUsers, userId]);
    setShowUserSelect(false);
  };

  const handleRemoveUser = (userId) => {
    setAssignedUsers(assignedUsers.filter(id => id !== userId));
  };

  const handleSave = () => {
    const statusChanged = status !== task.status;
    const assignedChanged = JSON.stringify(assignedUsers.sort()) !== JSON.stringify(task.assignedTo.map(u => u.id).sort());

    onSave({
      statusChanged,
      assignedChanged,
      newStatus: status,
      newAssignedUsers: assignedUsers,
    });
  };

  return (
    <div className="modal-backdrop" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <button className="modal-close" onClick={onClose}>×</button>

        <h2 className="task-title">{task.title}</h2>
        <p className="task-description">{task.description}</p>

        <div className="task-meta">
          <div className="status-selector">
            <select 
              value={status} 
              onChange={(e) => setStatus(e.target.value)}
              className={`status-select status-${status.toLowerCase()}`}
            >
              <option value="OPEN">OPEN</option>
              <option value="IN_PROGRESS">IN_PROGRESS</option>
              <option value="BLOCKED">BLOCKED</option>
              <option value="DONE">DONE</option>
            </select>
          </div>
          <span className={`priority priority-${task.priority.toLowerCase()}`}>
            {task.priority}
          </span>
        </div>

        <div className="task-section">
          <label>Created by:</label>
          <div className="user-card">
            {task.createdBy.username}
          </div>
        </div>

        <div className="task-section">
          <label>Assigned to:</label>
          <div className="assigned-users">
            {assignedUsers.map(userId => {
              const user = allUsers.find(u => u.id === userId);
              return user ? (
                <div key={userId} className="user-card-editable">
                  {user.username}
                  <button 
                    className="remove-user" 
                    onClick={() => handleRemoveUser(userId)}
                  >
                    ×
                  </button>
                </div>
              ) : null;
            })}
            
            {availableUsers.length > 0 && (
              <div className="add-user-container">
                {!showUserSelect ? (
                  <button 
                    className="add-user-btn" 
                    onClick={() => setShowUserSelect(true)}
                  >
                    +
                  </button>
                ) : (
                  <div className="user-select-dropdown">
                    <select onChange={(e) => handleAddUser(Number(e.target.value))} defaultValue="">
                      <option value="" disabled>Select user...</option>
                      {availableUsers.map(user => (
                        <option key={user.id} value={user.id}>
                          {user.username}
                        </option>
                      ))}
                    </select>
                    <button onClick={() => setShowUserSelect(false)}>Cancel</button>
                  </div>
                )}
              </div>
            )}
          </div>
        </div>

        <button className="save-btn" onClick={handleSave}>
          Save changes
        </button>
      </div>
    </div>
  );
}