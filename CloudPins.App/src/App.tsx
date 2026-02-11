import { Route, Routes } from "react-router-dom";

import IndexPage from "@/pages/index";
import Profile from "@/pages/profile";
import Login from "./pages/login";
import Register from "./pages/register";
import Feed from "./pages/feed";

function App() {
  return (
    <Routes>
      <Route element={<IndexPage />} path="/" />
      <Route element={<Profile />} path="/profile" />
      <Route element={<Login />} path="/login" />
      <Route element={<Register />} path="/register" />
      <Route element={<Feed />} path="/feed" />
    </Routes>
  );
}

export default App;
