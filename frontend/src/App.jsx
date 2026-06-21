import { BrowserRouter, Routes, Route } from "react-router-dom";
import Home from "./user_features/pages/Home";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/tourist" element={<Home />}/>
            </Routes>
        </BrowserRouter>
    );
}

export default App;

//local:5000/{tourist, owner, admin}
