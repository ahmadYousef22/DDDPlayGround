async function loadGreeting() {
  const output = document.getElementById('output');
  output.textContent = 'Loading...';
  try {
    const res = await fetch('./assets/greeting.json', { cache: 'no-store' });
    if (!res.ok) throw new Error(`HTTP ${res.status}`);
    const data = await res.json();
    output.textContent = JSON.stringify(data, null, 2);
  } catch (err) {
    console.error(err);
    output.textContent = 'Failed to load. Check console.';
  }
}

document.getElementById('loadBtn')?.addEventListener('click', loadGreeting);

// Expose something to play with in devtools
window.app = { loadGreeting };
