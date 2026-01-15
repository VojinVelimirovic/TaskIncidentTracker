import { useState, useEffect } from "react";
import Navbar from "../components/layout/Navbar";
import TaskModal from "../components/layout/TaskModal";
import { getAllTasks, assignTask, changeTaskStatus } from "../api/tasks";
import { getAllUsers } from "../api/auth";
import "../styles/global.css";
import "../styles/MyTasks.css";

export default function AllTasks() {
  const [tasks, setTasks] = useState([]);
  const [allUsers, setAllUsers] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [selectedTask, setSelectedTask] = useState(null);

  const pageSize = 6;

  useEffect(() => {
    fetchData();
  }, [page]);

  const fetchData = async () => {
    setLoading(true);
    setError("");

    try {
      const [tasksResponse, usersResponse] = await Promise.all([
        getAllTasks(page, pageSize),
        getAllUsers()
      ]);

      if (tasksResponse.data) {
        setTasks(tasksResponse.data);
        setTotalPages(Math.ceil(tasksResponse.totalCount / pageSize));
      } else {
        setTasks([]);
        setTotalPages(1);
      }

      if (usersResponse.data) {
        setAllUsers(usersResponse.data);
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleTaskClick = (task) => {
    setSelectedTask(task);
  };

  const handleCloseModal = () => {
    setSelectedTask(null);
  };

  const handleSaveTask = async ({ statusChanged, assignedChanged, newStatus, newAssignedUsers }) => {
    try {
      if (statusChanged) {
        await changeTaskStatus(selectedTask.id, newStatus);
      }

      if (assignedChanged) {
        await assignTask(selectedTask.id, newAssignedUsers);
      }

      await fetchData();
      setSelectedTask(null);
    } catch (err) {
      setError(err.message);
    }
  };

  if (loading) {
    return (
      <div>
        <Navbar />
        <div className="container">
          <p>Loading tasks...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div>
        <Navbar />
        <div className="container">
          <p className="error">{error}</p>
        </div>
      </div>
    );
  }

  return (
    <div>
      <Navbar />

      <div className="container">
        <h1>All Tasks</h1>

        {tasks.length === 0 ? (
          <p className="no-tasks">No tasks available</p>
        ) : (
          <>
            <div className="tasks-grid">
              {tasks.map((task) => (
                <div 
                  key={task.id} 
                  className="task-card task-card-clickable" 
                  onClick={() => handleTaskClick(task)}
                >
                  <h2 className="task-title">{task.title}</h2>
                  <p className="task-description">{task.description}</p>

                  <div className="task-meta">
                    <span className={`status status-${task.status.toLowerCase()}`}>
                      {task.status}
                    </span>
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
                      {task.assignedTo.map((user) => (
                        <div key={user.id} className="user-card">
                          {user.username}
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              ))}
            </div>

            {totalPages > 1 && (
              <div className="pagination">
                {Array.from({ length: totalPages }, (_, i) => i + 1).map((pageNum) => (
                  <button
                    key={pageNum}
                    onClick={() => setPage(pageNum)}
                    className={page === pageNum ? "active" : ""}
                  >
                    {pageNum}
                  </button>
                ))}
              </div>
            )}
          </>
        )}
      </div>

      {selectedTask && (
        <TaskModal
          task={selectedTask}
          allUsers={allUsers}
          onClose={handleCloseModal}
          onSave={handleSaveTask}
        />
      )}
    </div>
  );
}