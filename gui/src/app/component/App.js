import React, { useState, useEffect } from "react";
import "../style/App.css";
import PersonImage from "../resources/img/Person.png";
import apiService from "../service/apiService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDeleteLeft, faPlus } from "@fortawesome/free-solid-svg-icons";

const URL = "http://localhost:5015";
const ENDPOINT = "api/tasks";
const service = apiService(URL, ENDPOINT);

function App() {
  const [tasks, setTasks] = useState([]);
  const [newTask, setNewTask] = useState("");

  useEffect(() => {
    const fetchTasks = async () => {
      const response = await service.getAll();
      setTasks(response);
    };
    fetchTasks();
  }, []);

  const handleAddTask = async () => {
    const updatedTasks = [...tasks];
    updatedTasks.push({ title: newTask });
    setTasks(updatedTasks);
    setNewTask("");
    await service.post({ title: newTask });
  };

  const handleDeleteTask = async (index) => {
    const updatedTasks = [...tasks];
    const taskId = tasks[index].id;
    await service.delete(taskId);
    updatedTasks.splice(index, 1);
    setTasks(updatedTasks);
  };

  const handleComplete = async (taskId) => {
    try {
      const updatedTasks = tasks.map((task) =>
        task.id === taskId ? { ...task, isDone: !task.isDone } : task
      );
      setTasks(updatedTasks);
  
      await service.patch(taskId, { isDone: updatedTasks.isDone });
    } catch (error) {
    }
  };

  return (
    <div className="App">
      <div className="App-header">
        <div className="content-container">
          <h1 className="title">Organize Your Day with Ease!</h1>
          <img className="imgPerson" src={PersonImage} alt="Person" />
        </div>
        <div className="container">
          <div className="to-do-list">
            <h2>To Do List</h2>
            <input
              type="text"
              placeholder="Add new task."
              autoComplete="off"
              autoFocus
              value={newTask}
              onChange={(e) => setNewTask(e.target.value)}
            />
            <div className="btn_container">
              <button onClick={handleAddTask}>
                <FontAwesomeIcon icon={faPlus} />
              </button>
            </div>
          </div>
          <ul className="tasklist">
            {tasks.map((task, index) => (
              <li
                key={task.id}
                style={{ backgroundColor: task.isDone ? "green" : "white" }}
                onClick={() => handleComplete(task.id)}
              >
                {task.title}
                <button
                  className="deleteButton"
                  onClick={() => handleDeleteTask(index)}
                >
                  <FontAwesomeIcon icon={faDeleteLeft} />
                </button>
              </li>
            ))}
          </ul>
        </div>
      </div>
    </div>
  );
}

export default App;
