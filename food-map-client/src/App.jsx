import { useState } from 'react'
import Login from './pages/Login'
import Dashboard from './pages/Dashboard'

function App() {
    const [owner, setOwner] = useState(null)

    if (!owner) return <Login onLogin={setOwner} />
    return <Dashboard owner={owner} onLogout={() => setOwner(null)} />
}

export default App