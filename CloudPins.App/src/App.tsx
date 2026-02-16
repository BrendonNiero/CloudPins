import { Route, Routes } from "react-router-dom";

import IndexPage from "@/pages/index";
import Profile from "@/pages/profile";
import Login from "./pages/login";
import Register from "./pages/register";
import Feed from "./pages/feed";
import { ProtectedRoute } from "./routes/ProtectedRoute";
import Explorer from "./pages/explorer";

function App() {
  return (
    <Routes>
      <Route element={<IndexPage />} path="/" />
      <Route element={<Login />} path="/login" />
      <Route element={<Register />} path="/register" />
      <Route element={
        <ProtectedRoute>
          <Profile />
        </ProtectedRoute>
      } path="/profile" />
      <Route element={
        <ProtectedRoute>
          <Feed />
        </ProtectedRoute>
      } path="/feed" />

      <Route element={
        <ProtectedRoute>
          <Explorer />
        </ProtectedRoute>
      } path="/explorar/:id" />
    </Routes>
  );
}

export default App;
