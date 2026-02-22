import { Route, Routes } from "react-router-dom";

import IndexPage from "@/pages/index";
import Profile from "@/pages/profile";
import Login from "./pages/login";
import Register from "./pages/register";
import Feed from "./pages/feed";
import { ProtectedRoute } from "./routes/ProtectedRoute";
import Explorer from "./pages/explorer";
import BoardPins from "./pages/boardPins";

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
      <Route element={
        <ProtectedRoute>
          <BoardPins />
        </ProtectedRoute>
      } path="/board/:id"/>
    </Routes>
  );
}

export default App;
