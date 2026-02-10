import { Route, Routes } from "react-router-dom";

import IndexPage from "@/pages/index";
import Profile from "@/pages/profile";
import Login from "./pages/login";

function App() {
  return (
    <Routes>
      <Route element={<IndexPage />} path="/" />
      <Route element={<Profile />} path="/profile" />
      <Route element={<Login />} path="/login" />
    </Routes>
  );
}

export default App;
