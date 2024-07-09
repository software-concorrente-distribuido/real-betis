import logo from './images/logo.jpeg'
import './App.css';
import githublogo from './images/github-mark-white.png'
import Header from './components/Header';
import Participantes from './components/Participantes';
import Footer from './components/Footer';

function App() {
  return (
    <div className="App">
    <Header />
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
            <a
                className="App-link"
                href="https://github.com/software-concorrente-distribuido/real-betis"
                target="_blank"
                rel="noopener noreferrer"
            >
                <img src={githublogo} className='Github-logo' alt="githublogo" />
                Github
            </a>
        <Participantes />
      </header> 
      <Footer />
    </div>
  );
}

export default App;
