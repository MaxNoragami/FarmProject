import "./App.css";

function App() {
  const title = "App Component";
  const views = 20;
  const numUsers = [1, 2, 3, 4, 5];
  const user = { name: "John", surname: "Pork", age: 32 };

  return (
    <div>
      <div>{title}</div>
      <div>This has {views} views, I assume.</div>
      <div>{numUsers}</div>
      <div>Current user is {user.name + " " + user.surname};</div>
    </div>
  );
}

export default App;
