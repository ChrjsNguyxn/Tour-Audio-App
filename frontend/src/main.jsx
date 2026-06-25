import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'

/* tạm thời tắt cái này để chỉ render 1 lần
createRoot(document.getElementById('root')).render(
  <StrictMode>
    <App />
  </StrictMode>,
)
*/
createRoot(document.getElementById('root')).render(
  <App />
)
